using Caliburn.Metro;
using Caliburn.Metro.Core;
using Caliburn.Micro;
using PracticumEmailer.Ui.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;

namespace PracticumEmailer.Ui
{
    [SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class MefBootstrapper : CaliburnMetroCompositionBootstrapper<IShell>
    {
        private CompositionContainer _container;

        public MefBootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            var builder = new RegistrationBuilder();
            builder.ForTypesMatching(t => t.Name.EndsWith("ViewModel"))
                .Export()
                .SetCreationPolicy(CreationPolicy.NonShared);

            _container
                = new CompositionContainer(
                    new AggregateCatalog(
                        AssemblySource.Instance.Select(x => new AssemblyCatalog(x, builder))
                            .OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new MetroWindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);

            LogManager.GetLog = type => new DebugLog(type);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = _container.GetExportedValues<object>(contract);

            var enumerable = exports as IList<object> ?? exports.ToList();
            if (enumerable.Any())
                return enumerable.First();

            throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
        }

        protected override void BuildUp(object instance)
        {
            _container.SatisfyImportsOnce(instance);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            CheckForTemplates();
            DisplayRootViewFor<IShell>();
        }

        private static void CheckForTemplates()
        {
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                Settings.Default.BaseDataDirectory)))
            {
                var tempFile = Path.GetTempFileName();
                File.WriteAllBytes(tempFile, Resources.PracticumEmailer);
                ZipFile.ExtractToDirectory(tempFile,
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            }
        }
    }
}
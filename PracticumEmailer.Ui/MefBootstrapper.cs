using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.Registration;
using System.Linq;
using System.Windows;
using Caliburn.Micro;

namespace PracticumEmailer.Ui
{
    public class MefBootstrapper : BootstrapperBase
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

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_container);

            _container.Compose(batch);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            IEnumerable<object> exports = _container.GetExportedValues<object>(contract);

            if (exports.Any())
                return exports.First();

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
            DisplayRootViewFor<IShell>();
        }
    }
}
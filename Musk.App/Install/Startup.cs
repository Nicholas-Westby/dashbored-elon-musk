namespace Musk.App.Install
{

    // Namespaces.
    using Properties;
    using System.Linq;
    using System.Web.Configuration;
    using System.Xml;
    using umbraco.cms.businesslogic.packager;
    using Umbraco.Core;
    using Umbraco.Core.Configuration.Dashboard;

    /// <summary>
    /// Handles startup events.
    /// </summary>
    public class Startup : ApplicationEventHandler
    {

        #region Event Handlers

        /// <summary>
        /// Application started event handler.
        /// </summary>
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication,
            ApplicationContext applicationContext)
        {
            EnsureDashboardConfigExists();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Ensures the "Musk" section exists in the dashboard.config.
        /// </summary>
        private void EnsureDashboardConfigExists()
        {
            var exists = DashboardExists();
            if (!exists)
            {
                var doc = new XmlDocument();
                var actionXml = Resources.TransformDashboard;
                doc.LoadXml(actionXml);
                PackageAction.RunPackageAction("Musk", "Musk.TransformXmlFile", doc.FirstChild);
            }
        }

        /// <summary>
        /// Indicates whether or not the "MuskSection" exists in the dashboard.config.
        /// </summary>
        private bool DashboardExists()
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            var dashboard = config.GetSection("umbracoConfiguration/dashBoard")
                as IDashboardSection;
            var hasMusk = dashboard.Sections.Any(x =>
                "MuskSection".InvariantEquals(x.Alias));
            return hasMusk;
        }

        #endregion

    }

}
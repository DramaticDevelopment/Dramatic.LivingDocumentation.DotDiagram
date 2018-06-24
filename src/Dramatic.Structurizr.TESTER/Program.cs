using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dramatic.LivingDocumentation.DotDiagram;
using Dramatic.Structurizr.IO.Dot;
using Structurizr;
using Structurizr.Core.Util;
using Structurizr.IO.PlantUML;

namespace Dramatic.Structurizr.TESTER
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var workspace = CreateBugbank();
                //var workspace = CreatFinancialRisk();

                StringBuilder sb = new StringBuilder();
                using (StringWriter writer = new StringWriter(sb))
                {
                    PlantUMLWriter plantWriter = new PlantUMLWriter();
                    plantWriter.Write(workspace, writer);
                }

                string uml = sb.ToString();
                    

/*
                DotWriter dotwriter = new DotWriter();
                String dotContent = dotwriter.Write(workspace);

                // and Create the image
                var imageFilePath = GraphVizDot.RenderImage(dotContent, "png");
                if (File.Exists(imageFilePath))
                {
                    Console.WriteLine($"Opening {imageFilePath}...");
                    System.Diagnostics.Process.Start(imageFilePath);
                }
 */
            }
            catch(Exception ex)
            {
                Console.WriteLine("{0}:\r\n{1}", ex.GetType().FullName, ex.Message);
                Console.ReadLine();
            }
          
        }

        private const string ExistingSystemTag = "Existing System";
        private const string BankStaffTag = "Bank Staff";
        private const string WebBrowserTag = "Web Browser";
        private const string MobileAppTag = "Mobile App";
        private const string DatabaseTag = "Database";
        private const string FailoverTag = "Failover";


        private const string AlertTag = "Alert";


        private static Workspace CreatFinancialRisk()
        {
            Workspace workspace = new Workspace("Financial Risk System", "This is a simple (incomplete) example C4 model based upon the financial risk system architecture kata, which can be found at http://bit.ly/sa4d-risksystem");
            Model model = workspace.Model;

            SoftwareSystem financialRiskSystem = model.AddSoftwareSystem("Financial Risk System", "Calculates the bank's exposure to risk for product X.");

            Person businessUser = model.AddPerson("Business User", "A regular business user.");
            businessUser.Uses(financialRiskSystem, "Views reports using");

            Person configurationUser = model.AddPerson("Configuration User", "A regular business user who can also configure the parameters used in the risk calculations.");
            configurationUser.Uses(financialRiskSystem, "Configures parameters using");

            SoftwareSystem tradeDataSystem = model.AddSoftwareSystem("Trade Data System", "The system of record for trades of type X.");
            financialRiskSystem.Uses(tradeDataSystem, "Gets trade data from");

            SoftwareSystem referenceDataSystem = model.AddSoftwareSystem("Reference Data System", "Manages reference data for all counterparties the bank interacts with.");
            financialRiskSystem.Uses(referenceDataSystem, "Gets counterparty data from");

            SoftwareSystem referenceDataSystemV2 = model.AddSoftwareSystem("Reference Data System v2.0", "Manages reference data for all counterparties the bank interacts with.");
            referenceDataSystemV2.AddTags("Future State");
            financialRiskSystem.Uses(referenceDataSystemV2, "Gets counterparty data from").AddTags("Future State");

            SoftwareSystem emailSystem = model.AddSoftwareSystem("E-mail system", "The bank's Microsoft Exchange system.");
            financialRiskSystem.Uses(emailSystem, "Sends a notification that a report is ready to");
            emailSystem.Delivers(businessUser, "Sends a notification that a report is ready to", "E-mail message", InteractionStyle.Asynchronous);

            SoftwareSystem centralMonitoringService = model.AddSoftwareSystem("Central Monitoring Service", "The bank's central monitoring and alerting dashboard.");
            financialRiskSystem.Uses(centralMonitoringService, "Sends critical failure alerts to", "SNMP", InteractionStyle.Asynchronous).AddTags(AlertTag);

            SoftwareSystem activeDirectory = model.AddSoftwareSystem("Active Directory", "The bank's authentication and authorisation system.");
            financialRiskSystem.Uses(activeDirectory, "Uses for user authentication and authorisation");

            ViewSet views = workspace.Views;
            SystemContextView contextView = views.CreateSystemContextView(financialRiskSystem, "Context", "An example System Context diagram for the Financial Risk System architecture kata.");
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            Styles styles = views.Configuration.Styles;
            financialRiskSystem.AddTags("Risk System");

            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff", FontSize = 34 });
            styles.Add(new ElementStyle("Risk System") { Background = "#550000", Color = "#ffffff" });
            //styles.Add(new ElementStyle(Tags.SoftwareSystem) { Width = 650, Height = 400, Background = "#801515", Shape = Shape.RoundedBox });
            //styles.Add(new ElementStyle(Tags.Person) { Width = 550, Background = "#d46a6a", Shape = Shape.Person });

            //styles.Add(new RelationshipStyle(Tags.Relationship) { Thickness = 4, Dashed = false, FontSize = 32, Width = 400 });
            styles.Add(new RelationshipStyle(Tags.Synchronous) { Dashed = false });
            styles.Add(new RelationshipStyle(Tags.Asynchronous) { Dashed = true });
            styles.Add(new RelationshipStyle(AlertTag) { Color = "#ff0000" });

            styles.Add(new ElementStyle("Future State") { Opacity = 30, Border = Border.Dashed });
            styles.Add(new RelationshipStyle("Future State") { Opacity = 30, Dashed = true });

            return workspace;
        }
        private static Workspace CreateBugbank()
        {
            Workspace workspace = new Workspace("Big Bank plc", "This is an example workspace to illustrate the key features of Structurizr, based around a fictional online banking system.");
            Model model = workspace.Model;
            ViewSet views = workspace.Views;

            model.Enterprise = new Enterprise("Big Bank plc");

            // people and software systems
            Person customer = model.AddPerson(Location.External, "Personal Banking Customer", "A customer of the bank, with personal bank accounts.");

            SoftwareSystem internetBankingSystem = model.AddSoftwareSystem(Location.Internal, "Internet Banking System", "Allows customers to view information about their bank accounts, and make payments.");
            customer.Uses(internetBankingSystem, "Uses");

            SoftwareSystem mainframeBankingSystem = model.AddSoftwareSystem(Location.Internal, "Mainframe Banking System", "Stores all of the core banking information about customers, accounts, transactions, etc.");
            mainframeBankingSystem.AddTags(ExistingSystemTag);
            internetBankingSystem.Uses(mainframeBankingSystem, "Uses");

            SoftwareSystem emailSystem = model.AddSoftwareSystem(Location.Internal, "E-mail System", "The internal Microsoft Exchange e-mail system.");
            emailSystem.AddTags(ExistingSystemTag);
            emailSystem.Delivers(customer, "Sends e-mails to");

            SoftwareSystem atm = model.AddSoftwareSystem(Location.Internal, "ATM", "Allows customers to withdraw cash.");
            atm.AddTags(ExistingSystemTag);
            atm.Uses(mainframeBankingSystem, "Uses");
            customer.Uses(atm, "Withdraws cash using");

            Person customerServiceStaff = model.AddPerson(Location.Internal, "Customer Service Staff", "Customer service staff within the bank.");
            customerServiceStaff.AddTags(BankStaffTag);
            customerServiceStaff.Uses(mainframeBankingSystem, "Uses");
            customer.InteractsWith(customerServiceStaff, "Asks questions to", "Telephone");

            Person backOfficeStaff = model.AddPerson(Location.Internal, "Back Office Staff", "Administration and support staff within the bank.");
            backOfficeStaff.AddTags(BankStaffTag);
            backOfficeStaff.Uses(mainframeBankingSystem, "Uses");

            // containers
            Container singlePageApplication = internetBankingSystem.AddContainer("Single-Page Application", "Provides all of the Internet banking functionality to customers via their web browser.", "JavaScript and Angular");
            singlePageApplication.AddTags(WebBrowserTag);
            Container mobileApp = internetBankingSystem.AddContainer("Mobile App", "Provides a limited subset of the Internet banking functionality to customers via their mobile device.", "Xamarin");
            mobileApp.AddTags(MobileAppTag);
            Container webApplication = internetBankingSystem.AddContainer("Web Application", "Delivers the static content and the Internet banking single page application.", "Java and Spring MVC");
            Container apiApplication = internetBankingSystem.AddContainer("API Application", "Provides Internet banking functionality via a JSON/HTTPS API.", "Java and Spring MVC");
            Container database = internetBankingSystem.AddContainer("Database", "Stores user registration information, hashed authentication credentials, access logs, etc.", "Relational Database Schema");
            database.AddTags(DatabaseTag);

            customer.Uses(webApplication, "Uses", "HTTPS");
            customer.Uses(singlePageApplication, "Uses", "");
            customer.Uses(mobileApp, "Uses", "");
            webApplication.Uses(singlePageApplication, "Delivers", "");
            apiApplication.Uses(database, "Reads from and writes to", "JDBC");
            apiApplication.Uses(mainframeBankingSystem, "Uses", "XML/HTTPS");
            apiApplication.Uses(emailSystem, "Sends e-mail using", "SMTP");

            // components
            // - for a real-world software system, you would probably want to extract the components using
            // - static analysis/reflection rather than manually specifying them all
            Component signinController = apiApplication.AddComponent("Sign In Controller", "Allows users to sign in to the Internet Banking System.", "Spring MVC Rest Controller");
            Component accountsSummaryController = apiApplication.AddComponent("Accounts Summary Controller", "Provides customers with a summary of their bank accounts.", "Spring MVC Rest Controller");
            Component securityComponent = apiApplication.AddComponent("Security Component", "Provides functionality related to signing in, changing passwords, etc.", "Spring Bean");
            Component mainframeBankingSystemFacade = apiApplication.AddComponent("Mainframe Banking System Facade", "A facade onto the mainframe banking system.", "Spring Bean");

            apiApplication.Components.Where(c => "Spring MVC Rest Controller".Equals(c.Technology)).ToList().ForEach(c => singlePageApplication.Uses(c, "Uses", "HTTPS"));
            apiApplication.Components.Where(c => "Spring MVC Rest Controller".Equals(c.Technology)).ToList().ForEach(c => mobileApp.Uses(c, "Uses", "HTTPS"));
            signinController.Uses(securityComponent, "Uses");
            accountsSummaryController.Uses(mainframeBankingSystemFacade, "Uses");
            securityComponent.Uses(database, "Reads from and writes to", "JDBC");
            mainframeBankingSystemFacade.Uses(mainframeBankingSystem, "Uses", "XML/HTTPS");

            model.AddImplicitRelationships();

            // deployment nodes and container instances
            DeploymentNode developerLaptop = model.AddDeploymentNode("Developer Laptop", "A developer laptop.", "Microsoft Windows 10 or Apple macOS");
            DeploymentNode apacheTomcat = developerLaptop.AddDeploymentNode("Docker Container - Web Server", "A Docker container.", "Docker")
                .AddDeploymentNode("Apache Tomcat", "An open source Java EE web server.", "Apache Tomcat 8.x", 1, DictionaryUtils.Create("Xmx=512M", "Xms=1024M", "Java Version=8"));
            apacheTomcat.Add(webApplication);
            apacheTomcat.Add(apiApplication);

            developerLaptop.AddDeploymentNode("Docker Container - Database Server", "A Docker container.", "Docker")
                .AddDeploymentNode("Database Server", "A development database.", "Oracle 12c")
                .Add(database);

            developerLaptop.AddDeploymentNode("Web Browser", "", "Google Chrome, Mozilla Firefox, Apple Safari or Microsoft Edge").Add(singlePageApplication);

            DeploymentNode customerMobileDevice = model.AddDeploymentNode("Customer's mobile device", "", "Apple iOS or Android");
            customerMobileDevice.Add(mobileApp);

            DeploymentNode customerComputer = model.AddDeploymentNode("Customer's computer", "", "Microsoft Windows or Apple macOS");
            customerComputer.AddDeploymentNode("Web Browser", "", "Google Chrome, Mozilla Firefox, Apple Safari or Microsoft Edge").Add(singlePageApplication);

            DeploymentNode bigBankDataCenter = model.AddDeploymentNode("Big Bank plc", "", "Big Bank plc data center");

            DeploymentNode liveWebServer = bigBankDataCenter.AddDeploymentNode("bigbank-web***", "A web server residing in the web server farm, accessed via F5 BIG-IP LTMs.", "Ubuntu 16.04 LTS", 4, DictionaryUtils.Create("Location=London and Reading"));
            liveWebServer.AddDeploymentNode("Apache Tomcat", "An open source Java EE web server.", "Apache Tomcat 8.x", 1, DictionaryUtils.Create("Xmx=512M", "Xms=1024M", "Java Version=8"))
                .Add(webApplication);

            DeploymentNode liveApiServer = bigBankDataCenter.AddDeploymentNode("bigbank-api***", "A web server residing in the web server farm, accessed via F5 BIG-IP LTMs.", "Ubuntu 16.04 LTS", 8, DictionaryUtils.Create("Location=London and Reading"));
            liveApiServer.AddDeploymentNode("Apache Tomcat", "An open source Java EE web server.", "Apache Tomcat 8.x", 1, DictionaryUtils.Create("Xmx=512M", "Xms=1024M", "Java Version=8"))
                .Add(apiApplication);

            DeploymentNode primaryDatabaseServer = bigBankDataCenter.AddDeploymentNode("bigbank-db01", "The primary database server.", "Ubuntu 16.04 LTS", 1, DictionaryUtils.Create("Location=London"))
                .AddDeploymentNode("Oracle - Primary", "The primary, live database server.", "Oracle 12c");
            primaryDatabaseServer.Add(database);

            DeploymentNode secondaryDatabaseServer = bigBankDataCenter.AddDeploymentNode("bigbank-db02", "The secondary database server.", "Ubuntu 16.04 LTS", 1, DictionaryUtils.Create("Location=Reading"))
                .AddDeploymentNode("Oracle - Secondary", "A secondary, standby database server, used for failover purposes only.", "Oracle 12c");
            ContainerInstance secondaryDatabase = secondaryDatabaseServer.Add(database);

            model.Relationships.Where(r => r.Destination.Equals(secondaryDatabase)).ToList().ForEach(r => r.AddTags(FailoverTag));
            Relationship dataReplicationRelationship = primaryDatabaseServer.Uses(secondaryDatabaseServer, "Replicates data to", "");
            secondaryDatabase.AddTags(FailoverTag);

            // views/diagrams
            //SystemLandscapeView systemLandscapeView = views.CreateSystemLandscapeView("SystemLandscape", "The system landscape diagram for Big Bank plc.");
            //systemLandscapeView.AddAllElements();
            //systemLandscapeView.PaperSize = PaperSize.A5_Landscape;

            SystemContextView systemContextView = views.CreateSystemContextView(internetBankingSystem, "SystemContext", "The system context diagram for the Internet Banking System.");
            //systemContextView.EnterpriseBoundaryVisible = false;
            systemContextView.AddNearestNeighbours(internetBankingSystem);
            systemContextView.PaperSize = PaperSize.A5_Landscape;

            ContainerView containerView = views.CreateContainerView(internetBankingSystem, "Containers", "The container diagram for the Internet Banking System.");
            containerView.Add(customer);
            containerView.AddAllContainers();
            containerView.Add(mainframeBankingSystem);
            containerView.Add(emailSystem);
            containerView.PaperSize = PaperSize.A5_Landscape;

            ComponentView componentView = views.CreateComponentView(apiApplication, "Components", "The component diagram for the API Application.");
            componentView.Add(mobileApp);
            componentView.Add(singlePageApplication);
            componentView.Add(database);
            componentView.AddAllComponents();
            componentView.Add(mainframeBankingSystem);
            componentView.PaperSize = PaperSize.A5_Landscape;

            // dynamic diagrams and deployment diagrams are not available with the Free Plan
            DynamicView dynamicView = views.CreateDynamicView(apiApplication, "SignIn", "Summarises how the sign in feature works in the single-page application.");
            dynamicView.Add(singlePageApplication, "Submits credentials to", signinController);
            dynamicView.Add(signinController, "Calls isAuthenticated() on", securityComponent);
            dynamicView.Add(securityComponent, "select * from users where username = ?", database);
            dynamicView.PaperSize = PaperSize.A5_Landscape;

            DeploymentView developmentDeploymentView = views.CreateDeploymentView(internetBankingSystem, "DevelopmentDeployment", "An example development deployment scenario for the Internet Banking System.");
            //developmentDeploymentView.Environment = "Development";
            developmentDeploymentView.Add(developerLaptop);
            developmentDeploymentView.PaperSize = PaperSize.A5_Landscape;

            DeploymentView liveDeploymentView = views.CreateDeploymentView(internetBankingSystem, "LiveDeployment", "An example live deployment scenario for the Internet Banking System.");
            //liveDeploymentView.Environment = "Live";
            liveDeploymentView.Add(bigBankDataCenter);
            liveDeploymentView.Add(customerMobileDevice);
            liveDeploymentView.Add(customerComputer);
            liveDeploymentView.Add(dataReplicationRelationship);
            liveDeploymentView.PaperSize = PaperSize.A5_Landscape;

            // colours, shapes and other diagram styling
            Styles styles = views.Configuration.Styles;
            styles.Add(new ElementStyle(Tags.Element) { Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#438dd5" });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#85bbf0", Color = "#000000" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Shape = Shape.Person, FontSize = 22 });
            styles.Add(new ElementStyle(ExistingSystemTag) { Background = "#999999" });
            styles.Add(new ElementStyle(BankStaffTag) { Background = "#999999" });
            styles.Add(new ElementStyle(WebBrowserTag) { Shape = Shape.WebBrowser });
            styles.Add(new ElementStyle(MobileAppTag) { Shape = Shape.MobileDeviceLandscape });
            styles.Add(new ElementStyle(DatabaseTag) { Shape = Shape.Cylinder });
            styles.Add(new ElementStyle(FailoverTag) { Opacity = 25 });
            styles.Add(new RelationshipStyle(FailoverTag) { Opacity = 25, Position = 70 });

            return workspace;
        }
    }
}

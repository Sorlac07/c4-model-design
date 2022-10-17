﻿using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            RenderModels();
        }

        static void RenderModels()
        {
            const long workspaceId = 77477;
            const string apiKey = "4099c9d0-46ef-49d0-88b3-9700ab20ea67";
            const string apiSecret = "2be2395d-d775-4200-aebb-f96004b9fbfd";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);

            Workspace workspace = new Workspace("Software Design & Patterns - C4 Model - TakeMeHome", "Allows users to bring products from the US to Peru");

            ViewSet viewSet = workspace.Views;

            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem ImportItSystem = model.AddSoftwareSystem("ImportIt System", "Allows users to bring.");
            SoftwareSystem googleMaps = model.AddSoftwareSystem("Google Maps", "Platform that offers a REST API of georeferential information.");
            SoftwareSystem PaymentSystem = model.AddSoftwareSystem("Payment Gateway System", "Allow the customers to make payments.");
            SoftwareSystem EmailSystem = model.AddSoftwareSystem("E-mail System", "The internal Microsoft Exchange  e-mail System.");

            Person traveler = model.AddPerson("Traveler", "A traveler with a registred account.");
            Person ImportCustomer = model.AddPerson("Customer", "A web app user with a registred account. ");

            traveler.Uses(ImportItSystem, "Search for products to import and offer her services");
            ImportCustomer.Uses(ImportItSystem, "Search for Tarvelers and select one to contract their services using");

            ImportItSystem.Uses(PaymentSystem, "Makes payment using");
            ImportItSystem.Uses(googleMaps, "Uses the Google API");
            ImportItSystem.Uses(EmailSystem, "Send e-mail using");

            EmailSystem.Delivers(traveler,"Sends emails to");
            EmailSystem.Delivers(ImportCustomer, "Sends emails to");
            


            // Tags
            traveler.AddTags("Traveler");
            ImportCustomer.AddTags("Customer");
            ImportItSystem.AddTags("SistemaMonitoreo");
            googleMaps.AddTags("GoogleMaps");
            PaymentSystem.AddTags("PaymentSystem");

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Traveler") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("Customer") { Background = "#aa60af", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaMonitoreo") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMaps") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("PaymentSystem") { Background = "#2f95c7", Color = "#ffffff", Shape = Shape.RoundedBox });
            
            SystemContextView contextView = viewSet.CreateSystemContextView(ImportItSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A4_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // 2. Diagrama de Contenedores
            

            Container webApplication = ImportItSystem.AddContainer("Web App", "Permite a los usuarios visualizar un dashboard con el resumen de toda la información del traslado de los lotes de vacunas.", "React");
            Container landingPage = ImportItSystem.AddContainer("Landing Page", "", "React");
            Container apiRest = ImportItSystem.AddContainer("API REST", "API Rest", "NodeJS (NestJS) port 8080");

            Container loginContext = ImportItSystem.AddContainer("Log In", "Allows customers and travelers to log into the account", "NodeJS (NestJS)");
            Container reviewContext = ImportItSystem.AddContainer("Review", "Customer rates the Traveler", "NodeJS (NestJS)");
            Container messageContext = ImportItSystem.AddContainer("Message", "Allows communication betweeen customers and travelers", "NodeJS (NestJS)");
            Container SeachProductContext = ImportItSystem.AddContainer("Search Products", "Customer locates order in real time", "NodeJS (NestJS)");
            Container PayOrderContext = ImportItSystem.AddContainer("Pay Order", "Allow customers to pay for pending orders", "JNodeJS (NestJS)");
                       
            Container database = ImportItSystem.AddContainer("Database", "", "Oracle");

            traveler.Uses(webApplication, "Consulta");
            traveler.Uses(landingPage, "Consulta");

            
            ImportCustomer.Uses(webApplication, "Consulta");
            ImportCustomer.Uses(landingPage, "Consulta");

            webApplication.Uses(apiRest, "API Request", "JSON/HTTPS");

            apiRest.Uses(loginContext, "", "");
            apiRest.Uses(reviewContext, "", "");
            apiRest.Uses(messageContext, "", "");
            apiRest.Uses(SeachProductContext, "", "");
            apiRest.Uses(PayOrderContext, "", "");


            loginContext.Uses(database, "", "");
            reviewContext.Uses(database, "", "");
            SeachProductContext.Uses(database, "", "");
            SeachProductContext.Uses(googleMaps, "API Request", "JSON/HTTPS");
            messageContext.Uses(database, "", ""); 
            PayOrderContext.Uses(database, "", "");
            PayOrderContext.Uses(PaymentSystem, "API Request", "J");
       

            // Tags
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            apiRest.AddTags("APIRest");
            database.AddTags("Database");

            string contextTag = "Context";

            loginContext.AddTags(contextTag);
            reviewContext.AddTags(contextTag);
            messageContext.AddTags(contextTag);
            SeachProductContext.AddTags(contextTag);
            PayOrderContext.AddTags(contextTag);
    

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("APIRest") { Shape = Shape.RoundedBox, Background = "#0000ff", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle(contextTag) { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });

            ContainerView containerView = viewSet.CreateContainerView(ImportItSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();

            // 3.1 Diagrama de Componentes (PayOrder Context)

            Component domainLayer = PayOrderContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");

            Component PaymentController = PayOrderContext.AddComponent("PaymentController", "Controls all transactions", "NodeJS (NestJS) REST Controller");
            Component OrderPaymentSystem = PayOrderContext.AddComponent("OrderPaymentSystem", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");
            Component PaymentHistoryObserver = PayOrderContext.AddComponent("PaymentHistoryObserver", "Saves the customer's payment history", "NestJS Component");
            //Component vaccineLoteRepository = PayOrderContext.AddComponent("VaccineLoteRepository", "Información de lote de vacunas", "NestJS Component");

            //Component locationRepository = PayOrderContext.AddComponent("LocationRepository", "Ubicación del vuelo", "NestJS Component");

            Component PaymentSystemFacade = PayOrderContext.AddComponent("Payment System Facade", "", "NestJS Component");

            apiRest.Uses(PaymentController, "", "JSON/HTTPS");
            PaymentController.Uses(OrderPaymentSystem, "Invoca métodos de monitoreo");

            OrderPaymentSystem.Uses(domainLayer, "Uses", "");
            OrderPaymentSystem.Uses(PaymentSystemFacade, "Uses");
            OrderPaymentSystem.Uses(PaymentHistoryObserver, "", "");
            //OrderPaymentSystem.Uses(vaccineLoteRepository, "", "");
            //OrderPaymentSystem.Uses(locationRepository, "", "");

            PaymentHistoryObserver.Uses(database, "", "");
            //vaccineLoteRepository.Uses(database, "", "");
            //locationRepository.Uses(database, "", "");

           // //locationRepository.Uses(googleMaps, "", "JSON/HTTPS");

            PaymentSystemFacade.Uses(PaymentSystem, "JSON/HTTPS");
            
            // Tags
            domainLayer.AddTags("DomainLayer");
            PaymentController.AddTags("PaymentController");
            OrderPaymentSystem.AddTags("OrderPaymentSystem");
            PaymentHistoryObserver.AddTags("PaymentHistoryObserver");
            //vaccineLoteRepository.AddTags("VaccineLoteRepository");
           // locationRepository.AddTags("LocationRepository");
            PaymentSystemFacade.AddTags("PaymentSystemFacade");
            
            styles.Add(new ElementStyle("DomainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("OrderPaymentSystem") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("MonitoringDomainModel") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("FlightStatus") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentHistoryObserver") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("PaymentSystemFacade") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView componentView = viewSet.CreateComponentView(PayOrderContext, "Components", "Component Diagram");

            componentView.PaperSize = PaperSize.A4_Landscape;
            componentView.Add(webApplication);
            componentView.Add(apiRest);
            componentView.Add(database);
            componentView.Add(PaymentSystem);
            //componentView.Add(googleMaps);
            componentView.AddAllComponents();




            // 3.2 Diagrama de Componentes (SearchOrder Context)

            domainLayer = SeachProductContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");

            Component LocationGPSController = SeachProductContext.AddComponent("LocationGPSController", "Controls all transactions", "NodeJS (NestJS) REST Controller");
            Component SeachProductSystem = SeachProductContext.AddComponent("SeachProductSystem", "Provee métodos para el monitoreo, pertenece a la capa Application de DDD", "NestJS Component");
            Component ProductPlataformRepository = SeachProductContext.AddComponent("Product Platform Repository", "S", "NestJS Component");
            //Component vaccineLoteRepository = SeachProductContext.AddComponent("VaccineLoteRepository", "Información de lote de vacunas", "NestJS Component");

            //Component locationRepository = SeachProductContext.AddComponent("LocationRepository", "Ubicación del vuelo", "NestJS Component");


            apiRest.Uses(LocationGPSController, "", "JSON/HTTPS");

            SeachProductSystem.Uses(domainLayer, "Uses", "");
            SeachProductSystem.Uses(PaymentSystemFacade, "Uses");
            SeachProductSystem.Uses(ProductPlataformRepository, "", "");
            

            ProductPlataformRepository.Uses(database, "", "");

            LocationGPSController.Uses(googleMaps, "", "JSON/HTTPS");
            LocationGPSController.Uses(SeachProductSystem, "", "JSON/HTTPS");

            // Tags
          
            SeachProductSystem.AddTags("SeachProductSystem");
            ProductPlataformRepository.AddTags("ProductPlataformRepository");
            LocationGPSController.AddTags("LocationGPSController");
            domainLayer.AddTags("domainLayer");
       
          
            styles.Add(new ElementStyle("LocationGPSController") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("SeachProductSystem") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("ProductPlataformRepository") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            styles.Add(new ElementStyle("domainLayer") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });
            

            ComponentView componentView1 = viewSet.CreateComponentView(SeachProductContext, "Components1", "Component Diagram1");

            componentView1.PaperSize = PaperSize.A4_Landscape;
            componentView1.Add(webApplication);
            componentView1.Add(apiRest);
            componentView1.Add(database);
            componentView1.Add(googleMaps);
            componentView1.AddAllComponents();


            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }





    }
}
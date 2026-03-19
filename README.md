# Piattaforma Ticketing

Piattaforma di ticketing composta da:

- **Backend REST API** in **.NET 10 (target LTS richiesto)** con approccio **DDD**, **SOLID** e separazione a layer.
- **Frontend Angular** organizzato per feature e pronto per integrazione con la API.
- **Integrazione Azure e Azure DevOps** per autenticazione, configurazione applicativa, storage e collegamento dei ticket a work item/user stories.

## Struttura

```text
src/
  backend/
    Ticketing.Api/
    Ticketing.Application/
    Ticketing.Domain/
    Ticketing.Infrastructure/
  frontend/
```

## Backend

### Layer DDD

- **Domain**: entità, value object, enum, repository contracts.
- **Application**: casi d'uso, DTO, interfacce, orchestrazione.
- **Infrastructure**: persistenza, servizi Azure, repository implementation.
- **Api**: endpoint REST, middleware, configurazione DI.

### Azure readiness

Il backend è predisposto per:

- `Azure.Identity` per `DefaultAzureCredential`
- `Microsoft.Extensions.Configuration.AzureAppConfiguration`
- `Azure.Storage.Blobs`
- JWT Bearer con Microsoft Entra ID
- **Azure DevOps Work Item Tracking API** per creare **User Stories** collegate al ticket

### Azure DevOps linkage

Quando dici "collegata ad Azure", in questo progetto significa anche che un ticket può opzionalmente:

- creare una **User Story** su Azure DevOps
- salvare l'`id` del work item creato
- salvare l'URL del work item Azure DevOps
- mantenere il ticket applicativo allineato al tracciamento del lavoro di delivery

La sezione `AzureDevOps` in `appsettings.json` serve a configurare:

- `Organization`
- `Project`
- `PersonalAccessToken`
- `DefaultAreaPath`
- `DefaultIterationPath`

## Frontend Angular

Frontend Angular standalone con:

- architettura feature-based
- servizi HTTP dedicati
- componenti standalone
- modelli tipizzati
- form per ticket con opzione di creazione User Story in Azure DevOps
- environment per endpoint API

## Esempio payload API

```json
{
  "title": "Errore login portale",
  "description": "L'utente riceve 500 durante il login.",
  "createdBy": "mario.rossi@contoso.com",
  "azureDevOps": {
    "createUserStory": true,
    "title": "US - Gestire errore login portale",
    "description": "Analizzare e correggere l'errore 500 sul login.",
    "areaPath": "Ticketing\Support",
    "iterationPath": "Ticketing\Sprint 1",
    "assignedTo": "team.support@contoso.com"
  }
}
```

## Avvio consigliato

### Backend

Richiede SDK .NET 10 quando disponibile nell'ambiente:

```bash
cd src/backend
 dotnet restore
 dotnet build
 dotnet test
```

### Frontend

```bash
cd src/frontend
npm install
npm start
```

## Variabili di configurazione backend

Configurare `src/backend/Ticketing.Api/appsettings.json` e `appsettings.Development.json` con:

- `AzureAd`
- `AzureDevOps`
- `AzureResources`
- `ConnectionStrings:TicketingDb`

## Note

In questo ambiente non è presente il runtime/.NET SDK, quindi la soluzione backend è stata preparata strutturalmente ma non compilata qui.

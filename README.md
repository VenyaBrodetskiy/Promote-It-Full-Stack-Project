# Full-Stack application "Promote it"

This is a pet project, made in pair with [Mila Shurupova](https://github.com/milaShurupova) with main purpose to learn/practice frontend and backend technologies.

### What is the application about?

Idea of project - to create system to promote agenda for a better society.
The system's goal is to promote social campaigns. The means to do so:

- Involve onboarding business organizations that donate products
- Onboarding non-profit organizations that want to promote campaigns
- Onboarding social activists – users of Twitter that can promote those campaigns.

## Table of Contents

- [Project purpose](#project-purpose)
- [Architecture](#architecture)
- [Usage and examples](#usage-and-examples)
- [Contact](#contact)

## Project purpose

### Learn new technologies

- Frontend
  - Angular
  - Blazor WebAssembly
- Backend
  - ASP.NET Web Api with controller
  - ASP.NET Web Api (minimal)
  - Node.js
- Cloud
  - MsSQL database hosted on Azure cloud
- Tests
  - xUnit
- Devops
  - GitHub Actions (automate tests)

### Practice programming languages

- C#
- Typescript
- HTML
- CSS

### Practice code review

- GitHub

### Practice team work

- Jira

## Architecture

### Overall

![overall architecture](/Assets/architecture.jpg)

### Frontend

![architecture](/Assets/architecture_frontend.jpg)

### Backend Node.js

![architecture](/Assets/architecture_backend_nodejs.jpg)

### Backend C#

![backend C# architecture](/Assets/architecture_backend_csharp.jpg)

## Usage and examples

### The system users:

1. ProLobby Owner – the company representative. This user manages the system.
2. Non-Profit Organization Representative – the user that creates a campaign
3. Business Company Representative – the user that represents a company that donates products for campaigns
4. Social Activists – Twitter users that promote campaigns

### As social activist,

- I'd like to register to the system to earn money and to be able to use the money to buy products
- I'd like to tweet about a campaign, so I'll promote it and also earn money
- I'd like to buy products so I can spend the money I earned
- I'd like to know my earning status, so I know my balance
- I'd like to donate a product to my chosen campaign so that I can promote it (
    - I can earn money, buy a product and donate it to a campaign, so the campaign now has more products attached to it

![Screenshot of UI for SA](/Assets/***.jpg)

### As business owner,

- I'd like to register to the system so I can donate products
- I'd like to donate goods to a set of chosen campaigns 
    - I'll provide the number of products and the value (price in dollars) of each product that I provide to each campaign
- I'd like to get the list of users and products that I need to ship so I can supply them
- I'd like to inform the system that I sent a product to a user so that the system can finish the transaction

![Screenshot of UI for BO](/Assets/***.jpg)

### As non-profit organization,

- I'd like to register to the system to be able to create a campaign
    - I'll provide a name, email, and a link to my organization's website
- I'd like to create a campaign
    - I'll provide a link (URL) to the campaign landing page and the campaign hashtag

![Screenshot of UI for NPO](/Assets/***.jpg)

### As prolobby owner,

- I'd like the system to give a dollar to the social activist for each tweet that promotes a campaign and to each following retweet
    - The tweet has to have the link and hashtag to the campaign page
- I'd like the system to issue a tweet whenever a social activist uses the points to buy a product 
    - The tweet includes the Twitter handle of the social activist and the business company
- I'd like the system to provide the report about the following:
    - Campaigns
    - Users
    - Tweets

![Screenshot of UI for PO](/Assets/***.jpg)

## Contact

**Venya Brodetskiy**
[GitHub](https://github.com/VenyaBrodetskiy)
[LinkedIn](https://www.linkedin.com/in/brodetskiyveniamin/)

**Mila Shurupova**
[GitHub](https://github.com/milaShurupova)
[LinkedIn](https://www.linkedin.com/in/liudmilashurupova/)

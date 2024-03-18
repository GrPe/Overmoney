# Overmoney

Overmoney - web app for personal finances management

Inspiration: https://moneymanagerex.org/

## Features

- [ ] Ability do create account/login via email
- [x] Support for multiple wallets
- [x] Support for multiple currency
- [x] Detailed transaction log including date, payee, category etc.
- [ ] Ability to add attachment to each transaction (eg. invoices, photos)
- [ ] Dashboards to easy track current expenses
- [ ] Support for reccuring transaction
- [ ] Budget planner
- [ ] Weekly/Monthly summaries send to email

![features](docs/features.drawio.png)

### Nice to have

- [ ] Login via Google/Microsoft
- [ ] Support for mobile devices
- [ ] Splited transaction (multiple categories for single transaction)

## Roadmap

| Name                                                             | Deadline   | Status      |
| ---------------------------------------------------------------- | ---------- | ----------- |
| README + plan                                                    | 2024.03.04 | ✅ Done     |
| Features without dashboards & budget planner & files (API only)  | 2024.03.15 | ✅ Done     |
| Features (files, reccuring transactions, budget planner) + tests | 2024.03.30 | In progress |
| Database, Security, Observability, CI/CD (build)                 | 2024.04.14 | Todo        |
| CI/CD + IaC (containers, networking)                             | 2024.04.21 | Todo        |
| Frontend ?                                                       | 2024.05.12 | Todo        |
| dashboards, summaries                                            | 2024.05.19 | Todo        |
| Nice to have feature                                             | 2024.05.31 | Todo        |

## Technology

List of technologies, frameworks, libraries used for implementation:

- [.NET 8.0](https://dotnet.microsoft.com/en-us/) (platform)
- [Docker](https://www.docker.com/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/) - ORM
- [Posgresql](https://www.postgresql.org.pl/) - database engine
- more in future...

## Containers

![containers](docs/containers.png)

## Things to learn

List of technologies, frameworks, libraries that I want to learn/test:

### Frameworks/Libraries

- [ ] ASP.NET Core (Security, OAuth)
- [ ] Dapper
- [ ] Worker Services / Hangfire
- [ ] Integration/E2E Tests

### Infra

- [ ] CI/CD (GitHub) including automatic build, test, deployment, database migrations
- [ ] IaC (Bicep)
- [ ] Observability: Loki, Grafana, Prometheus, Jaeger, "sth for alerting"
- [ ] Hashicorp Vault
- [ ] RabbitMQ
- [ ] Azure Containers, networking

## How to run

todo

# Hexachess

Hexachess is an implementation of Gliński's hexagonal chess in ASP.NET Core that I built during my second semester. The app features a real-time online-multiplayer mode that uses WebSockets (SignalR) but can also be played turn based on one screen.

## Setup

You can try out the app using Docker by running

```bash
git clone https://github.com/aarongoes/hexachess.git
cd ./hexachess
docker-compose up
```

and then navigating to [localhost:8080](localhost:8080) in your browser.

## Documatation

Documentation related to the requirements, architecture and the use of sql queries can be found here (in dutch)

- [Analysedocument](/docs/analysedocument.md)
- [Ontwerpdocument](/docs/ontwerpdocument.md)
- [Databasedocument](/docs/databasedocument.md)
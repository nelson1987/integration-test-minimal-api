# integration-test-minimal-api
## Criar projeto de WebApi
```sh
mkdir src
cd src
dotnet new web -o Economix.Api
cd ../
dotnet new gitignore
```
## Submeter para GitHub
```sh
git pull
git add .
git commit -m "Mensagem Commit"
git push
```
## Rodar Api
```sh
cd src/Economix.Api/
dotnet run
```
## Parar Api

```sh
ctrl+C
```

```sh
dotnet dev-certs https --trust
```
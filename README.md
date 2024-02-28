# integration-test-minimal-api
## Criar gitIgnore
```sh
dotnet new gitignore
```

## Criar projeto de WebApi
```sh
dotnet new gitignore
mkdir src
cd src
dotnet new web -o Economix.Api
cd Economix.Api
dotnet run
```

## Parar Api
```sh
ctrl+C
```

## Submeter para GitHub
```sh
git pull
git add .
git commit -m "Mensagem Commit"
git push
```

## Criar Projeto Test
```sh
mkdir test
cd test
dotnet new xunit -n Economix.Tests
cd Economix.Tests
dotnet tests
```

## Incluir frameworks em Tests
```sh

```

```sh
dotnet dev-certs https --trust
```
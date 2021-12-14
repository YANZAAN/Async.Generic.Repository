# Repository Pattern: Simple, Asynchronous and Generic
I wrote it for myself but who knows where it would appear...

## Benefits
1. Asynchronous (programmed with *``IAsyncEnumerable``, ``Task``*)
2. Generic (*T* - that's your stuff)
3. EF-connected (EF5)
4. Server-size pagination enabled
5. Usage explanation through tests

## Usage || restrictions
``git clone`` | Ctrl+C => Ctrl+V
  
No restriction for you. Take it, modify, use!
  
*Just set reference to this Github page, please*

## Test
Run ``dotnet test``

### ...with Docker:
Run ``docker build -t yanzaan/net5repositorytest . & docker run -it yanzaan/net5repositorytest``

### ...with Docker-Compose:
Run ``docker-compose up --build yanzaan/net5repositorytest``

## Contribute
Forks? Pulls (if I won't die from coronavirus infection)? You're welcome

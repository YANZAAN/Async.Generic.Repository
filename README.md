# Generic repository pattern implementations
I wrote it for myself but who knows where it would appear...

## Benefits
1. Work-ready (copy'n'paste | fork'n'upgrade)
3. EF-connected
4. Server-size pagination
5. Usage explanation through tests

## Usage and restrictions
No restrictions

## Repository files
1. Primitive/Simple/Stupid (Classic CRUD): ``src/NET.Repository/Primitive``
2. Full/Advanced (Specifications, Pagination, Bulk Insert / Update / Delete): ``src/NET.Repository/Full``

## Test
Run ``dotnet test``

### ...with Docker:
Run ``docker build -t yanzaan/netrepository . & docker run -it yanzaan/netrepository``

### ...with Docker-Compose:
Run ``docker-compose up --build``

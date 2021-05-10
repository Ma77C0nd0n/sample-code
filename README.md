# sample-code

## Included in this Repository

### Mapper


- Implementation of the `IOneToManyMapper` interface. 
- Optimised implementation for performance and readability. 
- Invalid operations (based on contract definition in the interface) should result in an exception. 
- Unit tests included.

### DocumentProcessingService

#### What Is It?

This is a Hosted Service written in `.NetCore 3.1`
This service periodically reads documents from a set of subdirectories, performs a keyword search on each file and persists results to an Entity Framework in-memory database.


### Included in this Repository:

## Mapper


- Implementation of the `IOneToManyMapper` interface. 
- Optimised implementation for performance and readability. 
- Invalid operations (based on contract definition in the interface) should result in an exception. 
- Unit tests included.

## DocumentProcessingService

### What Is It?

This is a Hosted Service written in `.NetCore 3.1`
This service periodically reads documents from a set of subdirectories, performs a keyword search on each file and persists results to an Entity Framework in-memory database.

### Decisions & trade-offs

- Some constants are used to save time. In an extended solution the fileshare location would be stored in a configuration file. 
- Currently the solution is only set-up to run locally, using an in-memory Database. This isn't the ideal database design but is also used to save time and provide a shared developer experience when running the solution locally. 
- I was reasonably lenient in the filename/contents validation 
- I choose to log warnings in a lot of cases rather than throwing when errors such that the solution could continue to run for all other files if one file was invalid.
- Ideally, there should be retry mechanisms for files we failed to process, then an archiving mechanism for failed files once the number of retries on a file has elapsed.
- With more time I would also improve the solution to track file states during processing, to avoid possible I/O exceptions

### Solution
Considering the request, and to facilitate customers regularly dropping files, I opted for a Hosted Service that would run periodically (currently every hour).
Within the Hosted Service we have a file orchestrator that is responsible for managing the flow of each job.

For each company directory - the file orchestrator:
1. Makes a call to `FileShareQuery` to retrieve filenames 
2. Utilises the `FileProcessingService` to process the file 

    1. This `FileProcessingService` makes a call to `FileShareQuery` to read and parse file data, performs and returns the lookup.
3. Persists document data via `LookupStore`
4. Calls on `FileDeletionRepository` to delete successfully processed files.

### How to install & run

- git clone https://github.com/Ma77C0nd0n/sample-code.git
- open solution within sample-code sub-directory
- set DocumentProcessingService.app as Startup Project
- Ensure `Fileshare_local/CompaniesDirectory` has companies with valid files (valid file can be found in `Temp` directory
- Build and Start project


### Technologies Used
- .NetCore 3.1
- EntityFrameworkCore in-memory database
- NUnit for testing


### Future changes
If I had more time, I would improve the following

- Error handling
- Config files/config server for fileshare location and action interval  
- Use an external, dedicated NoSQL database
- Improve Test coverage - for fileshare query and file deletion repo



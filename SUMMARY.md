# Backend Exercise


## Application

https://bnrposts20210419235009.azurewebsites.net/swagger/index.html

### Posts API

This endpoint is for the `Post` resource and supports all RESTful operations.

`GET /posts?filter.userId&skip&take` Search posts, filter by user

`GET /posts/{id}` Loads a post

`POST /posts` Creates a post

`PUT /posts/{id}` Updates a post

`DELETE /posts` Deletes a post

### Seeder API

The intent of this endpoint is to seed the database with a random set of post and user records to aid in testing.

`GET /seeder`

This operation queries all post and user data from the database grouped by user.

`POST /seeder`

This operation deletes all post and user data from the database. A random number of users is then created, and a random number from 0 to 20 posts is created for each user.

### Authentication

_Not covered in this iteration._

### Testing

Unit tests cover all testable units, except for SeedersController and boilerplate application startup files. No automated integration tests were written.

### Logging

_Not covered in this iteration._

## Database

Data is persisted to SQL Server database instance in Azure.

### Connection String

`Server=bnr-posts.database.windows.net;Database=bnr-posts;User ID=bnr-posts;Password=zR2}^Tn3nT5TC[SW`

### Schema

Entity: `Post`

Column Name | Data Type | Nullable | Index
| -- | -- | -- | -- |
| Id | Guid | | PK
| UserId | uniqueidentifier | | FK
| Title | varchar(191) | |
| Body | varchar(MAX) | |
| CreatedOn | date |  |  |  
<br />

Entity: `User`

Column Name | Data Type | Nullable | Index
-- | -- | -- | -- |
Id | Guid | | PK
Name | varchar(191) | |
Email | varchar(191) | |
Expertise | varchar(191) | |
CreatedOn | date |  |
<br />


## Additional Topics

### Future enhancements and technical debt

- Additional unit testing
- Automated integration tests
- Authentication
- Logging
- Client UI
- Dashboard UI


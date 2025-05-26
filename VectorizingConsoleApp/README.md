# Vectorizing Console App

This application is vectorizing console application built using .NET, Semantic Kernel, and Redis Vector Database. It allows users to generate embeddings for files within a given folder.

## Prerequisites

Before running this application, you'll need:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- [Docker](https://www.docker.com/products/docker-desktop/)
- A GitHub Personal Access Token (PAT) with access to models.inference.ai.azure.com

## Getting Started

### 1. Setting Up Redis with Docker

This application requires Redis with vector search capabilities. You can run it locally using Docker:

```bash
# Start Redis Stack (includes Redis Server + Redis Insight)
docker run -d --name redis-stack -p 6379:6379 -p 8001:8001 redis/redis-stack:latest

# Verify that Redis is running
docker ps
```

Alternative: If you only want the Redis server without Redis Insight:

```bash
docker run -d --name redis-stack-server -p 6379:6379 redis/redis-stack-server:latest
```

### 2. Configuring the Application

Open `Program.cs` and add your GitHub Personal Access Token (PAT):

```csharp
string githubPAT = "your-github-pat-here"; 
```

### 3. Building and Running the Application

```bash
# Navigate to the application directory
cd path/to/VectorizingConsoleApp

# Build the application
dotnet build

# Run the application
dotnet run
```

## Usage

Once the application is running:

1. The application will initialize the search engine and load text files from the `files` directory
2. When prompted with "What are you looking for:", enter your question about information within the given files
3. The application will embed your query and search for relevant information in the vector database
4. Results will be displayed with similarity scores and content snippets

Example queries can be found under the questions folder
- "Traditional Romanian dishes"
- "Tourist attractions in Timisoara"
- "How to get from the airport to the city center"

## Managing Redis

### Using Redis Insight

Redis Insight provides a graphical interface for managing your Redis instance:

1. Access Redis Insight at [http://localhost:8001](http://localhost:8001) in your browser
2. Connect to your Redis instance (it should auto-detect the local instance)
3. Explore keys, run commands, and monitor your Redis database

### Using Redis CLI

You can also connect to Redis using the Redis CLI:

```bash
# Connect to Redis CLI in the container
docker exec -it redis-stack redis-cli
```

Some useful Redis commands:

```
# List all keys
KEYS *

# Get information about a key
TYPE keyname

# Delete a key
DEL keyname

# Exit Redis CLI
CTRL+C
```

## Project Structure

- `Program.cs`: Main entry point and configuration
- `Embedder.cs`: Handles loading and embedding text files
- `Searcher.cs`: Implements the vector search functionality
- `Text.cs`: Defines the data model for the vector store
- `files/`: Contains text files about Timișoara to be indexed
- `questions/`: Sample questions for testing

## Troubleshooting

1. If Redis fails to start, ensure that ports 6379 and 8001 are not in use by other applications
2. If you encounter "Connection refused" errors, verify that the Redis container is running with `docker ps`
3. If the application fails to connect to Redis, check that the connection string in `Program.cs` matches your Redis configuration
4. To restart Redis if issues occur:
   ```bash
   docker restart redis-stack
   ```

## Additional Resources

- [Semantic Kernel Documentation](https://learn.microsoft.com/en-us/semantic-kernel/overview/)
- [Redis Vector Database](https://redis.io/docs/stack/search/reference/vectors/)
- [Docker Documentation](https://docs.docker.com/)
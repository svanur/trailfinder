# TrailFinder

A trail-finding application that helps users discover and navigate trails for running.

## Prerequisites

- Node.js 18+ and npm
- Docker Desktop
- Visual Studio Code or JetBrains Rider
- Supabase CLI (`npm install supabase`)

## Project Structure

trailfinder/ 

├── client/ 

# React frontend 

├── database/ 

# Database migrations and seeds 

├── types/ 

# Shared TypeScript types 

└── docs/ 

# Project documentation


## Quick Start

1. **Clone the repository**
   ```bash
   git clone [repository-url]
   cd trailfinder
   ```

2. **Set up local development environment**
   ```bash
   # Start Docker Desktop
   docker-compose up -d

   # Start local Supabase
   supabase start

   # Install dependencies
   npm install
   ```

3. **Set up environment variables**
   ```bash
   cp .env.example .env.local
   # Edit .env.local with your settings
   ```

4. **Start development server**
   ```bash
   npm run dev
   ```

## Development Stack

- **Frontend**
    - React 19.1.0
    - TypeScript 5.8.3
    - Mantine UI 8.0.2
    - TailwindCSS 4.1.8
    - React Router 7.6.2
    - Leaflet 1.9.4 (for maps)
    - Chart.js 4.4.9

- **Backend**
    - Supabase (Database & Authentication)
    - PostgreSQL 15+

- **Development Tools**
    - Vite 6.3.5
    - ESLint 9.25.0
    - Docker
    - Supabase CLI

## Development Workflow

1. **Database Changes**
   ```bash
   # Create new migration
   ./scripts/new-migration.ps1 -Name "feature_name"

   # Apply migrations
   ./scripts/deploy.ps1 -Environment development
   ```

2. **Running Tests**
   ```bash
   npm run test
   ```

3. **Code Quality**
   ```bash
   npm run lint
   npm run format
   ```

## Environment Setup

### Local Development

env 
VITE_SUPABASE_URL=[http://localhost:54321](http://localhost:54321) 
VITE_SUPABASE_ANON_KEY=[your-local-key]

#### supabase local development setup.

- API URL: http://127.0.0.1:54321
- GraphQL URL: http://127.0.0.1:54321/graphql/v1
- S3 Storage URL: http://127.0.0.1:54321/storage/v1/s3
- DB URL: postgresql://postgres:postgres@127.0.0.1:54322/postgres
- Studio URL: http://127.0.0.1:54323
- Inbucket URL: http://127.0.0.1:54324
- JWT secret: super-secret-jwt-token-with-at-least-32-characters-long
- anon key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6ImFub24iLCJleHAiOjE5ODM4MTI5OTZ9.CRXP1A7WOeoJeXxjNni43kdQwgnWNReilDMblYTn_I0
- service_role key: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZS1kZW1vIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImV4cCI6MTk4MzgxMjk5Nn0.EGIM96RAZx35lJzdJsyH-qQwv8Hdp7fsn3W0YpN81IU
- S3 Access Key: 625729a08b95bf1b7ff351a663f3a23c
- S3 Secret Key: 850181e4652dd023b7a98c58ae0d2d34bd487ee0cc3254aed6eda37307425907
- S3 Region: local


### Production

env VITE_SUPABASE_URL=https://[your-project].supabase.co

VITE_SUPABASE_ANON_KEY=[your-production-key]

## Contributing

1. Create a new branch (`feature/your-feature`)
2. Make changes
3. Run tests and linting
4. Submit a Pull Request

## Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run test` - Run tests
- `npm run lint` - Run ESLint
- `npm run format` - Format code with Prettier

## Deployment

[To be added as deployment strategy is finalized]

## License

MIT License


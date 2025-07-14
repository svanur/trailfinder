# TrailFinder

A trail-finding application that helps users discover and navigate trails for running.

## Prerequisites

- [Git](https://git-scm.com/downloads)
- [Node.js](https://nodejs.org/en/download) 18+ and npm
- [Docker Desktop](https://docs.docker.com/desktop/setup/install/windows-install/)
- [WSL](https://learn.microsoft.com/en-us/windows/wsl/install) (`wsl --install`)
- Visual Studio Code or JetBrains Rider or your favorite IDE
- [Supabase CLI](https://supabase.com/docs/guides/local-development/cli/getting-started) (`npm install supabase`)

## Project Structure

### trailfinder/

#### Unit tests for the solution

    ├── tests/
    
        ├── TrailFinder.Api.Tests/

        ├── TrailFinder.IntegrationTests/

#### Frontend for backend administration

    ├── TrailFinder.Admin/

#### API for the solution

    ├── TrailFinder.Api/

#### Application classes

    ├── TrailFinder.Application/

#### Core classes

    ├── TrailFinder.Core/

#### Database classes

    ├── TrailFinder.DB/

#### Infrastructure classes

    ├── TrailFinder.Infrastructure/

#### Frontend

    └── TrailFinder.Website/

# Project documentation

## Quick Start

1. **Clone the repository**

   ```bash
   git clone [repository-url]
   cd trailfinder
   ```

2. **Set up local development environment**

    ```bash
   # Install dependencies in TrailFinder.Web and TrailFinder.DB
   npm install
   
   # Start Docker Desktop
   docker-compose up -d

   # Start local Supabase in TrailFinder.DB
   supabase start
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
    - [React 19.1.0](https://react.dev/blog/2024/12/05/react-19)
    - [TypeScript](https://www.typescriptlang.org/) 5.8.3
    - [Mantine UI](https://mantine.dev/) 8.0.2
    - [TailwindCSS](https://tailwindcss.com/) 4.1.8
    - [React Router](https://reactrouter.com/) 7.6.2
    - [Leaflet](https://leafletjs.com/index.html) 1.9.4 (for maps)
    - [Chart.js](https://www.chartjs.org/docs/latest/) 4.4.9

- **Backend**
    - [Supabase (Database & Authentication)](https://supabase.com/docs/guides/auth)
    - [PostgreSQL](https://www.postgresql.org/) 15+

- **Development Tools**
    - [Vite](https://vite.dev/) 6.3.5
    - [ESLint](https://eslint.org/) 9.25.0
    - [Docker](https://www.docker.com/)
    - [Supabase CLI](https://supabase.com/docs/guides/local-development/cli/getting-started)

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
- JWT secret: (super-secret-jwt-token-with-at-least-32-characters-long)
- anon key: (anon-key)
- service_role key: (security-role-key)
- S3 Access Key: (s3-access-key)
- S3 Secret Key: (s3-secret-key)
- S3 Region: local

## GitHub and GPG key signing

Find the project on GitHub: [github.com/svanur/trailfinder](https://github.com/svanur/trailfinder)

### GPG keys

To set up GPG key signing for commits on GitHub, you'll need to follow these steps:

#### STEP 1: Generate a GPG Key (if you don’t have one yet)

```bash
gpg --full-generate-key
```

Choose:
- Key type: RSA and RSA
- Key size: 4096
- Expiration: Your choice, push enter if key does not expire
- Name and email: Use the same email as your GitHub account
- Choose O for Okay
- Enter a passphrase for protecting your new key

<code>Note</code>: Make sure the email matches exactly the email listed in your GitHub account under Settings → Emails.

You can view your current email using:

```bash
git config --global user.email
```

List your keys:

```bash
gpg --list-secret-keys --keyid-format LONG
```

Copy the GPG key ID (the string after: rsa4096/).

#### STEP 2: Export Your Public Key

```bash
gpg --armor --export YOUR_KEY_ID
```

Copy the entire output — This is your public key.

#### STEP 3: Add Your GPG Key to GitHub

1. Go to your GitHub profile.
2. Navigate to [Settings → SSH and GPG keys](https://github.com/settings/keys) → New GPG key.
3. Paste your public key and save.

#### Step 4: Configure Git to Use Your GPG Key

```bash
git config --global user.signingkey YOUR_KEY_ID
git config --global commit.gpgsign true
```

You can also sign individual commits with:

```bash
git commit -S -m "Your commit message"
```

#### Step 5: Test It

Make a commit and push it. On GitHub, you should see a "Verified" badge next to your commit.

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

## Hosting

Hosting wise, this is a hybrid solution

### Database: Supabase

The database is hosted on [Supabase](https://supabase.com/dashboard/)

### API: Render

The API is hosted on [Render](https://dashboard.render.com/)

### Web: Vercel

The API is hosted on [Vercel](https://vercel.com/login)

## Deployment

[To be added as deployment strategy is finalized]

## License

MIT License

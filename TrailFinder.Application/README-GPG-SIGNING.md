# GPG signing key

To create a GPG signing key for use with GitHub, follow these steps:

- Install GPG: If you haven't already, install GPG:
  - On Windows: Download Gpg4win
  - On macOS: Run brew install gnupg
  - On Linux: Install using your package manager (sudo apt install gnupg for

### Generate a new GPG key

Open a terminal and run:

```bash
gpg --full-generate-key
```

- Choose RSA and RSA as the key type.
- Use a key size of 4096 bits for security.
- Set an expiration date (or leave it indefinite).
- Enter your name and email address (use the same email linked to GitHub).
- Set a strong passphrase.

### List your GPG keys

After creation, list your key to get the key ID

```bash
gpg --list-secret-keys --keyid-format=long
```

### Export the public key

You need to add this public key to GitHub

```bash
gpg --armor --export <KEY_ID>
```
Copy the output and add it to GitHub under Settings > SSH and GPG keys > New GPG Key.

### Configure Git to use the GPG key

Set Git to sign your commits with the key:

using `--global` will add GPG signing for all solutions. 
Using `--local` can add GPG signing for a single solution only. 

```bash
git config --local user.signingkey <KEY_ID>
git config --local commit.gpgsign true
```

### Sign commits in your .NET project

When committing changes, use:

```bash
git commit -S -m "Your signed commit message"
```

If prompted, enter the passphrase.

### Verify the signed commit

After pushing to GitHub, verify the commit signature in the GitHub repository under commit details.

## Use Git Hooks for Automatic Signing

You can set up a Git pre-commit hook to automatically sign every commit. In your .NET project's .git/hooks/ directory, 
create or edit the pre-commit file:

```bash
#!/bin/sh
exec git commit -S "$@"
```

Make it executable:

```bash
chmod +x .git/hooks/pre-commit
```

### Configure Git for GPG Auto-Signing

You can configure Git globally to always sign commits:

```bash
git config --global commit.gpgsign true
```

This eliminates the need to manually add -S for each commit.

### Store GPG Passphrase Securely

Instead of entering the passphrase for every commit, you can use GPG Agent:

```bash
gpg --edit-key <KEY_ID>
```

Then enter:

```bash
trust
5
save
```

Use GPG Agent to cache the passphrase:

```bash
echo "default-cache-ttl 600" >> ~/.gnupg/gpg-agent.conf
echo "max-cache-ttl 7200" >> ~/.gnupg/gpg-agent.conf
gpgconf --reload gpg-agent
```

This caches the passphrase for easier automatic signing.

### Automate Signing in CI/CD

If using GitHub Actions, add this to your workflow:

```bash
steps:
  - name: Import GPG Key
    run: |
      echo "${{ secrets.GPG_PRIVATE_KEY }}" | gpg --import
      git config --global user.signingkey <KEY_ID>
      git config --global commit.gpgsign true
```

Ensure you store the private key in GitHub Secrets (GPG_PRIVATE_KEY).


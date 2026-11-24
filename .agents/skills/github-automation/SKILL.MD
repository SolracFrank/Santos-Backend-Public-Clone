---
name: github-automation
description: Manage GitHub repositories, issues, pull requests, and workflows through Rube MCP.
license: MIT
metadata:
  author: openakita
  version: "1.0.0"
requires:
  mcp: [rube]
---

# GitHub Automation

Confirm that the GitHub connection is active. Use `RUBE_SEARCH_TOOLS` before calling an operation to get its current schema.

- Identify the repository by owner and name. Check the target branch, permissions, and open checks before making a change.
- Paginate issue and pull request listings. Issue responses can include pull requests.
- Check mergeability and CI before merging. Confirm destructive actions with the user.

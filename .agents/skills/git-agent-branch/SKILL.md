---
name: git-agent-branch
description: Always work in a branch derived from the current branch plus -agent.
---

# Git Agent Branch Skill

Rule:

main -> main-agent
develop -> develop-agent
Frank -> Frank-agent

Workflow:

1. Detect current branch.
2. Create or switch to <current>-agent.
3. Never commit to the human branch.
4. Commit only after successful verification.
5. Report changed files and risk.

Commit format:

refactor: preserve behavior while improving <area>

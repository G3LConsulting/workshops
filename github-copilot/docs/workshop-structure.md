# GitHub Copilot Workshop - Structure Overview

## Workshop Details

| Item | Details |
|------|---------|
| **Duration** | 4 hours |
| **Format** | Presentation + Hands-on Lab |
| **Target Audience** | Developers (mixed skill levels) |
| **Tools Required** | VS Code + GitHub Copilot Extension |
| **Kata Repository** | [SupermarketReceipt-Refactoring-Kata](https://github.com/emilybache/SupermarketReceipt-Refactoring-Kata) |

---

## Agenda

### Part 1: Foundations (1h 30min)

| Time | Duration | Topic | Type |
|------|----------|-------|------|
| 0:00 | 20 min | **Introduction to LLMs** | Presentation |
| | | What are LLMs, how they're trained | |
| | | Strengths & limitations | |
| | | Why this matters for developers | |
| 0:20 | 25 min | **Introduction to GitHub Copilot** | Presentation |
| | | History and evolution | |
| | | Available models (GPT-4o, Claude, Gemini) | |
| | | Clients and interfaces | |
| | | Pricing tiers | |
| 0:45 | 30 min | **Ask / Edit / Agent Modes** | Presentation + Demo |
| | | Deep dive into each mode | |
| | | When to use which mode | |
| | | Live demonstrations | |
| 1:15 | 15 min | ☕ **Break** | |

### Part 2: Mastering Copilot (60min)

| Time | Duration | Topic | Type |
|------|----------|-------|------|
| 1:30 | 25 min | **Context is King** | Presentation + Demo |
| | | How Copilot uses context | |
| | | Reference syntax (#file, @workspace, etc.) | |
| | | Custom instructions file | |
| | | Best practices for better output | |
| 1:55 | 20 min | **Fine-tuning GitHub Copilot** | Presentation |
| | | Prompt files and libraries | |
| | | Custom agents | |
| | | Extensions ecosystem | |
| | | Enterprise features | |
| 2:15 | 15 min | ☕ **Break** | |

### Part 3: Hands-on Lab (1h 35min)

| Time | Duration | Topic | Type |
|------|----------|-------|------|
| 2:35 | 10 min | **Lab Setup** | Hands-on |
| | | Clone kata repository | |
| | | VS Code configuration | |
| | | Copilot verification | |
| 2:45 | 20 min | **Exercise 1: Code Exploration** | Hands-on |
| | | Using Ask mode to understand code | |
| | | Identifying code smells | |
| 3:05 | 30 min | **Exercise 2: Test Generation** | Hands-on |
| | | Generating unit tests with Copilot | |
| | | Using /tests command | |
| | | Approval testing setup | |
| 3:35 | 25 min | **Exercise 3: Refactoring** | Hands-on |
| | | Using Edit mode for refactoring | |
| | | Safe refactoring with tests | |
| 4:00 | 10 min | **Wrap-up & Q&A** | Discussion |

---

## Prerequisites for Participants

### Required Software

- [ ] VS Code (latest version)
- [ ] GitHub Copilot extension installed
- [ ] GitHub Copilot Chat extension installed
- [ ] Node.js 18+ (LTS recommended)
- [ ] npm or yarn
- [ ] Git

### Required Accounts

- [ ] GitHub account with Copilot access (Individual, Business, Enterprise Or Education plan)

### Pre-workshop Setup

```bash
# Clone the kata repository
git clone https://github.com/emilybache/SupermarketReceipt-Refactoring-Kata.git

# Navigate to TypeScript version
cd SupermarketReceipt-Refactoring-Kata/typescript

# Install dependencies
npm install

# Verify tests run
npm test
```

---

## Materials Included

| File | Description |
|------|-------------|
| `presentation.md` | Main MARP presentation (markdown) |
| `SPEAKER-NOTES.md` | Detailed speaker notes per slide |
| `HANDS-ON-LAB.md` | Step-by-step lab guide for participants |
| `copilot-instructions.md` | Example custom instructions file |
| `WORKSHOP-STRUCTURE.md` | This document |

---

## Room Setup Checklist

- [ ] Projector/screen for presentation
- [ ] Stable internet connection
- [ ] Each participant has VS Code ready
- [ ] Kata repository pre-cloned (backup: USB drives)
- [ ] Copilot licenses verified for all participants

---

## Facilitator Tips

1. **Mixed skill levels**: Start each section with basics, then go deeper
2. **Live coding**: Always narrate what you're doing
3. **Pause for questions**: After each major section
4. **Troubleshooting**: Have a co-facilitator help with individual issues during labs
5. **Time buffer**: The schedule includes buffer time - use it for deeper Q&A if ahead

---

## Post-Workshop Resources

Share with participants after the workshop:

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Copilot Chat Cheat Sheet](https://docs.github.com/en/copilot/reference/cheat-sheet)
- [Emily Bache's Refactoring Katas](https://github.com/emilybache)

# Week 5 — Learning & Research Log

## 1. Group project component allocation — open-source project research

This week's task required each team member to research a component (Authentication,
Authorization, Bootstrap theme, 3-tier architecture, Data models) and develop an individual POC,
with all POCs later integrated into a shared group project built on an existing open-source
codebase. Before component work could start, the team needed to agree on which open-source
project to base the group project on.

I spent time evaluating candidate open-source projects, including eShopOnWeb, before settling on
and proposing **Piranha CMS** to the team. Posted the following evaluation to the team chat:

> Hey everyone, I've spent quite a lot of time looking through different open-source projects,
> and I think I've found a potentially suitable one for our assignment: Piranha CMS.
>
> Before we decide, I just want to make sure I've understood the assignment requirements
> correctly.
>
> As I understand it, the task is roughly:
> - We choose an existing open-source project as our group project.
> - Each team member works on a different component / area to research and develop.
> - Each person develops a POC (Proof of Concept) individually to demonstrate that their chosen
>   technical approach/component is feasible.
> - The POCs are then integrated or re-implemented into the final group project.
>
> So, the POC does not necessarily mean developing a completely new feature. It is more like a
> small-scale implementation/prototype used to verify that a particular technical approach
> works, which can then be applied to the final group project.
>
> For the project itself, I found Piranha CMS, which seems to fit these requirements quite well.
>
> Its main technology stack includes:
> - .NET 8 / ASP.NET Core
> - C#
> - Entity Framework Core
> - ASP.NET Core MVC / Razor Pages
> - REST API
> - Bootstrap
> - Vue.js
> - Database support through EF Core
>
> It also has a modular and extensible architecture, which seems suitable for us to work on
> different components and then integrate them into the project.
>
> Importantly, it supports ASP.NET Core MVC and Razor Pages, which can be used as the
> Presentation Layer within a three-tier architecture, so I think it can be adapted well to the
> technical requirements mentioned by the lecturer.
>
> I've looked through quite a lot of different projects before settling on this one because I
> wanted to avoid projects that are too old, too small, purely tutorial projects, or mainly
> frameworks rather than actual open-source applications. Piranha seems to be a proper,
> established open-source CMS while still having a modern technology stack and good
> extensibility.

**Status:** Team decision on the final open-source project (and my specific component
allocation from the list above) was still pending at the end of this week — to be confirmed and
followed up on next week.

## 2. Technical study this week: .NET, Docker, Navicat

In parallel with the project-selection research, continued hands-on work with the same
technologies relevant to the group's likely stack (ASP.NET Core / EF Core), using the individual
BookStoreApp task as the practical vehicle:

**.NET / ASP.NET Core**
- Reviewed the classic `Startup.cs` pattern (`ConfigureServices` / `Configure` methods) as
  distinct from the newer minimal-hosting `Program.cs` style that's the default in .NET 8 —
  relevant because some course tasks and older open-source codebases (including CMS-style
  projects like Piranha) still use or reference this structure.
- Practised wiring `IHostBuilder` / `UseStartup<Startup>()` manually where a project needs the
  classic pattern instead of the default template.

**Docker**
- Ran a real SQL Server (Linux) container via Docker Desktop rather than a substitute database,
  to match course tasks that specifically require SQL Server.
- Learned the Apple Silicon (M-series) consideration: SQL Server's official image is x86-only, so
  it needs to run under emulation via `--platform linux/amd64` plus Docker Desktop's Rosetta
  setting — otherwise the container fails to start on an M-series Mac.

**Navicat**
- Set up Navicat as a substitute for SQL Server Management Studio (SSMS), which is Windows-only
  and can't run on macOS, to connect to and inspect the Dockerised SQL Server instance (tables,
  data, migration history).

## Next steps
- Follow up with the team on final project decision and my specific component allocation.
- Once confirmed, begin the individual POC for that component.

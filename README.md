# Kanban Project

A full-stack Kanban task management application built with:

- ASP.NET Core (Web API)
- PostgreSQL (via Entity Framework Core)
- Next.js 15 (React 19)
- Tailwind CSS + shadcn/ui for UI components

## Backend (.NET + PostgreSQL)

### Required Tools

- [.NET 8 SDK]
- [PostgreSQL]

### NuGet Packages to Install

Install these packages via NuGet or CLI:

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.NETCore.App
dotnet add package Npgsql
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

### configuration
Update your appsettings.json to match your PostgreSQL credentials:
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost:<port>; Username=<pgUser>; Password=<pgPassword>;Database=<dbName>"
}


## Front End
Frontend (Next.js + TailwindCSS + shadcn/ui)

### required 
Node.js (v18+ recommended)

### Install Dependencies
npm install

### Development Server
npm run dev

### UI Components Setup with shadcn/ui
!install if needed
[select, card, button, input, badge, usetoast, textarea]
pnpm dlx shadcn add <install table one by one>



Back end:
- INstall NUgget Package: 
	. Microsoft.EntityFrameworkCore
	. Microsoft.NETCore.App 
	. Npgsql
	. Npgsql.EntityFrameworkCore.PostgreSQL
- Update Database as yours ("ConnectionStrings": {
  "DefaultConnection": "Host=localhost:_port_; Username=_pgName_; Password=_pgPwd_ ;Database=_BaseName_"
},)

Front End:
- npm install

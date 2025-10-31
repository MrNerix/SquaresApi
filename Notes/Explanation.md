# Explanation of My Approach

This document explains how I approached the assignment, my thought process, and how I prioritized each part of the work.

---

## Starting Out

After creating an empty .NET 8 Minimal API project, I spent some time analyzing the requirements to decide on an efficient order of implementation.

I chose to work from the highest-priority functional requirements down, while keeping the bonus points (like tests and database persistence) in mind for later.  
My focus was to first get a fully working API using in-memory data, and then replace temporary parts (like the storage) with more permanent implementations.

---

## My 7-Step Plan (in order of importance)

### 1. Initial Setup
Created the base project and folder structure (`Models`, `Storage`).  
Set up DTOs (data transfer objects) and a temporary in-memory storage implementation.  
* This provided a clean foundation with separation of concerns before writing any endpoints.

---

### 2. POST - Import a list of points
Implemented the endpoint for importing multiple `{x, y}` points at once.  
Focused on validation and duplicate prevention.

---

### 3. POST — Add a single point
Added support for inserting one point at a time.  
Returned:
- 201 Created when new  
- 409 Conflict when the point already existed

---

### 4. DELETE — Delete a point
Implemented deletion of a point by coordinates.  
Chose to make the operation idempotent, always returning 204 No Content, even if the point didn’t exist.

---

### 5. GET — Retrieve identified squares
To satisfy the requirement “Use existing algorithms/libraries to identify squares (don’t reinvent the wheel)”,  
I implemented the square detection logic using the "NetTopologySuite (NTS)" library.

#### Why NTS:
- It’s a well-known open-source geometry library for .NET.
- Provides built-in geometry types (Point, Polygon, Coordinate).
- Offers ready-to-use methods like ".IsRectangle" for shape validation.
- Reduces the risk of calculation or logic errors from manual geometry code.

#### How I Use It:
1. The program iterates through all unique combinations of 4 points.
2. For each group, it creates an NTS "Polygon" object.
3. It checks if the polygon is a rectangle ("polygon.IsRectangle").
4. If all sides are equal (verified manually), it’s identified as a square.

---

### 6. Setup Tests (Bonus)
I added a new test project `SquaresApi.Tests` using "xUnit" to verify the correctness of the square-detection algorithm.

I chose to rely on NetTopologySuite’s Polygon.IsRectangle, which detects only axis-aligned rectangles/squares.
Rotated squares are not included, I prioritized correctness and simplicity using the library’s built-in geometry.

### What’s tested:
- Detection of simple axis-aligned squares  
- Handling of duplicate points  
- Behavior when no squares exist  

All tests pass successfully with `dotnet test`.

---

### 7. Setup Database (Bonus)
I replaced the temporary in-memory storage with a simple SQLite database using Entity Framework Core (EF Core).

The project now uses a local SQLite database via EF Core for persistence. Data is stored in squares.db and automatically created on first run.

---

## Timeline

- **Start Time:** 12:00 AM  
- **Critical requirements done:** 15.10 (~3 hours)  
- **End Time:** 16.10 (~4 hours) + 20 minutes tesing it on another computer and fixing the instructions.

Each commit focuses on a single functional step.

---

## Summary of Approach

My overall focus:
- Building correct, maintainable code first.  
- Keeping the project structure clean and modular.  
- Following RESTful standards (proper status codes, idempotent endpoints).  
- Completing mandatory requirements before moving to bonus tasks.

---

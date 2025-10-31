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

---

### 4. DELETE — Delete a point
Implement deletion of a point by coordinates.  

---

### 5. GET — Retrieve identified squares
Implement square-detection.  

---

### 6. Setup Tests (Bonus)
Creat xUnit tests to validate the algorithm.  

---

### 7. Setup Database (Bonus)
Replac the in-memory storage with a local database.

---

## Timeline

- **Start Time:** 12:00 AM  
- **End Time:** ~

Each commit focuses on a single functional step.
---

## Summary of Approach

My overall focus:
- Building correct, maintainable code first.  
- Keeping the project structure clean and modular.  
- Following RESTful standards (proper status codes, idempotent endpoints).  
- Completing mandatory requirements before moving to bonus tasks.

---

# Project1_Report

> Text extract of Daniel Macharia's coursework. Original source: DOCX. Embedded images and Office metadata are omitted from this public copy; sensitive identifiers and credential examples are redacted. Wording is preserved except for template placeholders and privacy cleanup.

Project 1 – ShinraNet

This document lists all API endpoints created for the ShinraNet Project. I implemented the endpoint using Fast API and tested using the provided. Each endpoint corresponds to actions performed in the interface

User Endpoints

GET /users/?name={name}

Gets a user by username.

Used when logging in.

GET /users/{id}

Gets user by ID.

Used when opening profile page.

POST /users/

Creates a new user.

Used when creating account.

PUT /users/{id}

Edits user information.

Used in admin panel.

PATCH /users/{id}/admin

Changes admin status.

DELETE /users/{id}

Deletes user account.

Post Endpoints 

GET /posts/

Gets all posts.

Used on home page.

GET /posts/user/{id}

Gets posts by user.

POST /posts/

Creates new post.

PUT /posts/{id}

Edits post.

PATCH /posts/{id}/like

Adds like.

PATCH /posts/{id}/dislike

Adds dislike.

DELETE /posts/{id}

Deletes post.

Flow 1:

Log in as Cloud

Like post

Dislike post

Edit post

Logout

Endpoints used:

GET /users/?name=

GET /posts/

PATCH /posts/{id}/like

PATCH /posts/{id}/dislike

PUT /posts/{id}

GET /users/{id}

 Flow 2:

Create account

Login

Create post

Logout

End Points:

POST /users/

GET /users/?name=

POST /posts/

Flow 3:

Login as CaitSith

Delete post

Edit user

Admin change

Delete user

Logout

Endpoints:

DELETE /posts/{id}

PUT /users/{id}

GET /users/

PATCH /users/{id}/admin

DELETE /users/{id}

All endpoints were implemented using FastAPI. The frontedn why successfulkly called for each endpoint that were required, and all rubric flow completed successfully. IF NEED I CAN GO BACK AND PUT ALL OF THE SCHREEN SHOTS I DIDN’T WHAT TO MAKE IT EVERYLONG WORD DOCX.

from fastapi import FastAPI, HTTPException, Query
from fastapi.middleware.cors import CORSMiddleware
from pydantic import BaseModel
from typing import Optional, List
import sqlite3

DB_NAME = "social_media.db"

app = FastAPI(title="ShinraNet API")

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)


class User(BaseModel):
    id: Optional[int] = None
    username: str
    image_url: str
    is_admin: bool = False


class Post(BaseModel):
    id: Optional[int] = None
    user_id: int
    title: str
    post_text: str
    likes: int = 0


def get_connection():
    conn = sqlite3.connect(DB_NAME)
    conn.row_factory = sqlite3.Row
    return conn


def row_to_user(row):
    return {
        "id": row["id"],
        "username": row["username"],
        "image_url": row["image_url"],
        "is_admin": bool(row["is_admin"])
    }


def row_to_post(row):
    return {
        "id": row["id"],
        "user_id": row["user_id"],
        "title": row["title"],
        "post_text": row["post_text"],
        "likes": row["likes"]
    }


@app.get("/")
def root():
    return {"message": "ShinraNet API is running"}


# ---------------- USERS ----------------

@app.post("/users/")
def create_user(user: User):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute(
        "SELECT * FROM users WHERE username = ?",
        (user.username,)
    ).fetchone()

    if existing:
        conn.close()
        raise HTTPException(status_code=400, detail="Username already exists")

    cur.execute(
        "INSERT INTO users (username, image_url, is_admin) VALUES (?, ?, ?)",
        (user.username, user.image_url, int(user.is_admin))
    )
    conn.commit()
    conn.close()
    return "User created successfully"


@app.get("/users/", response_model=List[User])
def get_users(name: Optional[str] = Query(None)):
    conn = get_connection()
    cur = conn.cursor()

    if name is None or name.strip() == "":
        rows = cur.execute("SELECT * FROM users").fetchall()
    else:
        rows = cur.execute(
            "SELECT * FROM users WHERE username = ?",
            (name,)
        ).fetchall()

    conn.close()
    return [row_to_user(row) for row in rows]


@app.get("/users/{user_id}", response_model=User)
def get_user(user_id: int):
    conn = get_connection()
    cur = conn.cursor()

    row = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    conn.close()

    if not row:
        raise HTTPException(status_code=404, detail="User not found")

    return row_to_user(row)


@app.put("/users/{user_id}")
def update_user(user_id: int, user: User):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="User not found")

    duplicate = cur.execute(
        "SELECT * FROM users WHERE username = ? AND id != ?",
        (user.username, user_id)
    ).fetchone()
    if duplicate:
        conn.close()
        raise HTTPException(status_code=400, detail="Username already exists")

    cur.execute(
        """
        UPDATE users
        SET username = ?, image_url = ?, is_admin = ?
        WHERE id = ?
        """,
        (user.username, user.image_url, int(user.is_admin), user_id)
    )
    conn.commit()
    conn.close()
    return "User updated successfully"


@app.patch("/users/{user_id}")
def patch_user_name(user_id: int, name: str):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="User not found")

    duplicate = cur.execute(
        "SELECT * FROM users WHERE username = ? AND id != ?",
        (name, user_id)
    ).fetchone()
    if duplicate:
        conn.close()
        raise HTTPException(status_code=400, detail="Username already exists")

    cur.execute("UPDATE users SET username = ? WHERE id = ?", (name, user_id))
    conn.commit()
    conn.close()
    return "Username updated successfully"


@app.patch("/users/{user_id}/is_admin")
def patch_user_is_admin(user_id: int, is_admin: bool):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="User not found")

    cur.execute("UPDATE users SET is_admin = ? WHERE id = ?", (int(is_admin), user_id))
    conn.commit()
    conn.close()
    return "Admin status updated successfully"


@app.patch("/users/{user_id}/image_url")
def patch_user_image_url(user_id: int, image_url: str):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="User not found")

    cur.execute("UPDATE users SET image_url = ? WHERE id = ?", (image_url, user_id))
    conn.commit()
    conn.close()
    return "Image URL updated successfully"


@app.delete("/users/{user_id}")
def delete_user(user_id: int):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM users WHERE id = ?", (user_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="User not found")

    cur.execute("DELETE FROM posts WHERE user_id = ?", (user_id,))
    cur.execute("DELETE FROM users WHERE id = ?", (user_id,))
    conn.commit()
    conn.close()
    return "User deleted successfully"


# ---------------- POSTS ----------------

@app.post("/posts/")
def create_post(post: Post):
    conn = get_connection()
    cur = conn.cursor()

    user = cur.execute("SELECT * FROM users WHERE id = ?", (post.user_id,)).fetchone()
    if not user:
        conn.close()
        raise HTTPException(status_code=400, detail="User does not exist")

    cur.execute(
        """
        INSERT INTO posts (user_id, title, post_text, likes)
        VALUES (?, ?, ?, ?)
        """,
        (post.user_id, post.title, post.post_text, post.likes)
    )
    conn.commit()
    conn.close()
    return "Post created successfully"


@app.get("/posts/", response_model=List[Post])
def get_posts(title: Optional[str] = Query(None)):
    conn = get_connection()
    cur = conn.cursor()

    if title is None or title.strip() == "":
        rows = cur.execute("SELECT * FROM posts").fetchall()
    else:
        rows = cur.execute("SELECT * FROM posts WHERE title = ?", (title,)).fetchall()

    conn.close()
    return [row_to_post(row) for row in rows]


@app.get("/posts/user/{user_id}", response_model=List[Post])
def get_posts_by_user(user_id: int):
    conn = get_connection()
    cur = conn.cursor()

    rows = cur.execute("SELECT * FROM posts WHERE user_id = ?", (user_id,)).fetchall()
    conn.close()

    return [row_to_post(row) for row in rows]


@app.get("/posts/{post_id}", response_model=Post)
def get_post(post_id: int):
    conn = get_connection()
    cur = conn.cursor()

    row = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    conn.close()

    if not row:
        raise HTTPException(status_code=404, detail="Post not found")

    return row_to_post(row)


@app.put("/posts/{post_id}")
def update_post(post_id: int, post: Post):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    user = cur.execute("SELECT * FROM users WHERE id = ?", (post.user_id,)).fetchone()
    if not user:
        conn.close()
        raise HTTPException(status_code=400, detail="User does not exist")

    cur.execute(
        """
        UPDATE posts
        SET user_id = ?, title = ?, post_text = ?, likes = ?
        WHERE id = ?
        """,
        (post.user_id, post.title, post.post_text, post.likes, post_id)
    )
    conn.commit()
    conn.close()
    return "Post updated successfully"


@app.patch("/posts/{post_id}/title")
def patch_post_title(post_id: int, title: str):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    cur.execute("UPDATE posts SET title = ? WHERE id = ?", (title, post_id))
    conn.commit()
    conn.close()
    return "Post title updated successfully"


@app.patch("/posts/{post_id}/post_text")
def patch_post_text(post_id: int, post_text: str):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    cur.execute("UPDATE posts SET post_text = ? WHERE id = ?", (post_text, post_id))
    conn.commit()
    conn.close()
    return "Post text updated successfully"


@app.patch("/posts/{post_id}/increment_likes")
def increment_post_likes(post_id: int):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    cur.execute("UPDATE posts SET likes = likes + 1 WHERE id = ?", (post_id,))
    conn.commit()
    conn.close()
    return "Post likes incremented successfully"


@app.patch("/posts/{post_id}/decrement_likes")
def decrement_post_likes(post_id: int):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    cur.execute(
        "UPDATE posts SET likes = CASE WHEN likes > 0 THEN likes - 1 ELSE 0 END WHERE id = ?",
        (post_id,)
    )
    conn.commit()
    conn.close()
    return "Post likes decremented successfully"


@app.delete("/posts/{post_id}")
def delete_post(post_id: int):
    conn = get_connection()
    cur = conn.cursor()

    existing = cur.execute("SELECT * FROM posts WHERE id = ?", (post_id,)).fetchone()
    if not existing:
        conn.close()
        raise HTTPException(status_code=404, detail="Post not found")

    cur.execute("DELETE FROM posts WHERE id = ?", (post_id,))
    conn.commit()
    conn.close()
    return "Post deleted successfully"
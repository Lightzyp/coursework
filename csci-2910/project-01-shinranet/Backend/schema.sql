CREATE TABLE posts (
	id INTEGER NOT NULL, 
	user_id INTEGER, 
	title VARCHAR, 
	post_text VARCHAR, 
	likes INTEGER, 
	PRIMARY KEY (id), 
	FOREIGN KEY(user_id) REFERENCES users (id)
);

CREATE TABLE users (
	id INTEGER NOT NULL, 
	username VARCHAR, 
	image_url VARCHAR, 
	is_admin BOOLEAN, 
	PRIMARY KEY (id)
);

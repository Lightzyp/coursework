#lets us save and load data to a file in json format
import json

# gives us a queue we cna use
from collections import deque


class PasswordEntry:
    def __init__(self, site, username, password):
        self.site = site
        self.username = username
        self.password = password


    def to_dict(self):
        return {
            "username": self.username,
            "password": self.password
        }
    
    def __str__(self):
        return f"{self.site} | {self.username} / {self.password}"

#main password storage
passwords = {}
#to undo last added password
undo_stack = []
# to request password changes
change_queue = deque()

#Password Function
def view_passwords():
    if not password:
        print("No Password Saved")
    else:
        for site, info in password.items():
            print(f"{site}: {info ['username']} / {info ['password']}")
            

def add_password():
    site = input("Website:")
    username = input("Username:")
    password = input("Password:")
    entry = PasswordEntry(site, username, password)
    passwords[site] = entry.to_dict()
    undo_stack.append(site)
    print("Password added.")

def view_passwords():
    if not passwords:
        print("No passwords saved.")
    else:
        for site, info in passwords.items():
            print(f"{site}: {info['username']} / {info['password']}")

def undo_last_add():
    if undo_stack:
        site = undo_stack.pop()
        passwords.pop(site, None)
        print(f"Undid password for: {site}")
    else:
        print("Nothing to undo.")

def request_password_change():
    site = input("Enter site to request password change:")
    change_queue.append(site)
    print(f"Change request added for: {site}")

def view_change_requests():
    if not change_queue:
        print("No pending change requests.")
    else:
        print("change requests:")
    for site in change_queue:
        print(f" - {site}")

def save_passowrds():
    with open("password.json","W") as file:
        json.dump(passwords, file, indent=4)
    print("passwords saved to file.")

def load_passwords():
    global passwords
    try:
        with open("passwords.json", "r") as file:
            passwords = json.load(file)
        print("Passwords loaded from file.")
    except FileNotFoundError:
        print("No saved file found. Starting fresh.")

def show_menu():
    print("\n--- Password Manager Menu ---")
    print("1. Add Password")
    print("2. View Passwords")
    print("3. Undo Last Add")
    print("4. Request Password Change")
    print("5. View Change Requests")
    print("6. Save Passwords to File")
    print("7. Load Passwords from File")
    print("8. Quit")


def main():
    while True:
        show_menu()
        choice = input("Enter your choice (1–8): ")

        if choice == "1":
            add_password()
        elif choice == "2":
            view_passwords()
        elif choice == "3":
            undo_last_add()
        elif choice == "4":
            request_password_change()
        elif choice == "5":
            view_change_requests()
        elif choice == "6":
            save_passwords()
        elif choice == "7":
            load_passwords()
        elif choice == "8":
            print("Goodbye!")
            break
        else:
            print("Invalid option. Try again.")

if __name__ == "__main__":
    main()


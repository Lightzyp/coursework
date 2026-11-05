from sqlalchemy import create_engine, or_
from sqlalchemy.orm import sessionmaker
from data_model import Base, Pokemon

# Database setup
SQLALCHEMY_DATABASE_URL = "sqlite:///./data.db"

engine = create_engine(SQLALCHEMY_DATABASE_URL)
Base.metadata.create_all(bind=engine)

Session = sessionmaker(bind=engine)
session = Session()


# -----------------------------
# CREATE
# -----------------------------
def create_pokemon():
    try:
        dex = int(input("Enter Dex Number: "))
        name = input("Enter Name: ")
        type1 = input("Enter Type 1: ")
        type2 = input("Enter Type 2 (or press ENTER if none): ")
        gen = int(input("Enter Generation: "))

        new_pokemon = Pokemon(
            DexNumber=dex,
            Name=name,
            Type1=type1,
            Type2=type2 if type2 else None,
            Generation=gen
        )

        session.add(new_pokemon)
        session.commit()

        print(f"\n{name} added successfully!\n")

    except Exception as e:
        session.rollback()
        print("Error creating Pokémon:", e)


# -----------------------------
# READ ONE
# -----------------------------
def read_pokemon():
    try:
        print("\nSearch By:")
        print("1. ID")
        print("2. DexNumber")
        print("3. Name")
        print("4. Type")
        print("5. Generation")

        choice = input("Choose option: ")

        if choice == "1":
            value = int(input("Enter ID: "))
            result = session.query(Pokemon).filter(Pokemon.ID == value).first()

        elif choice == "2":
            value = int(input("Enter DexNumber: "))
            result = session.query(Pokemon).filter(Pokemon.DexNumber == value).first()

        elif choice == "3":
            value = input("Enter Name: ")
            result = session.query(Pokemon).filter(Pokemon.Name.ilike(f"%{value}%")).first()

        elif choice == "4":
            value = input("Enter Type: ")
            result = session.query(Pokemon).filter(
                or_(Pokemon.Type1.ilike(f"%{value}%"),
                    Pokemon.Type2.ilike(f"%{value}%"))
            ).first()

        elif choice == "5":
            value = int(input("Enter Generation: "))
            result = session.query(Pokemon).filter(Pokemon.Generation == value).first()

        else:
            print("Invalid choice.")
            return

        if result:
            print(result)
        else:
            print("No Pokémon found.")

    except Exception as e:
        print("Error reading Pokémon:", e)


# -----------------------------
# READ ALL
# -----------------------------
def read_all():
    try:
        results = session.query(Pokemon).all()
        for pokemon in results:
            print(pokemon)

        print(f"\nTotal Pokémon found: {len(results)}\n")

    except Exception as e:
        print("Error retrieving records:", e)


# -----------------------------
# UPDATE
# -----------------------------
def update_pokemon():
    try:
        pokemon_id = int(input("Enter ID of Pokémon to update: "))
        pokemon = session.query(Pokemon).filter(Pokemon.ID == pokemon_id).first()

        if not pokemon:
            print("Pokémon not found.")
            return

        print("Press ENTER to keep existing value.\n")

        dex = input(f"DexNumber ({pokemon.DexNumber}): ")
        name = input(f"Name ({pokemon.Name}): ")
        type1 = input(f"Type1 ({pokemon.Type1}): ")
        type2 = input(f"Type2 ({pokemon.Type2}): ")
        gen = input(f"Generation ({pokemon.Generation}): ")

        confirm = input("Are you sure you want to update? (y/n): ")
        if confirm.lower() != "y":
            print("Update cancelled.")
            return

        if dex:
            pokemon.DexNumber = int(dex)
        if name:
            pokemon.Name = name
        if type1:
            pokemon.Type1 = type1
        if type2:
            pokemon.Type2 = type2
        if gen:
            pokemon.Generation = int(gen)

        session.commit()

        print(f"{pokemon.Name} updated successfully!")

    except Exception as e:
        session.rollback()
        print("Error updating Pokémon:", e)


# -----------------------------
# DELETE
# -----------------------------
def delete_pokemon():
    try:
        pokemon_id = int(input("Enter ID of Pokémon to delete: "))
        pokemon = session.query(Pokemon).filter(Pokemon.ID == pokemon_id).first()

        if not pokemon:
            print("Pokémon not found.")
            return

        confirm = input(f"Are you sure you want to delete {pokemon.Name}? (y/n): ")
        if confirm.lower() != "y":
            print("Delete cancelled.")
            return

        session.delete(pokemon)
        session.commit()

        print(f"{pokemon.Name} deleted successfully!")

    except Exception as e:
        session.rollback()
        print("Error deleting Pokémon:", e)


# -----------------------------
# MENU
# -----------------------------
def menu():
    while True:
        print("\n===== Pokémon Database Menu =====")
        print("1. Create Pokémon")
        print("2. Read Pokémon")
        print("3. Read All Pokémon")
        print("4. Update Pokémon")
        print("5. Delete Pokémon")
        print("6. Exit")

        choice = input("Select option: ")

        if choice == "1":
            create_pokemon()
        elif choice == "2":
            read_pokemon()
        elif choice == "3":
            read_all()
        elif choice == "4":
            update_pokemon()
        elif choice == "5":
            delete_pokemon()
        elif choice == "6":
            print("Exiting program...")
            break
        else:
            print("Invalid selection.")


if __name__ == "__main__":
    menu()
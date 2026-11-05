import pandas as pd
import matplotlib.pyplot as plt


# this function loads the data from the CSV file and returns a DataFrame also a DataFrane is a table that you cna mnipulate.
def load_data():
    df = pd.read_csv("videogames.csv")
    return df


# this function filters games by specific publisher
#then sorts the game by alphabetically by name
def games_by_publisher(df, publisher_name):
    filtered = df[df["Publisher"] == publisher_name]
    sorted_games = filtered.sort_values(by="Name")
    return sorted_games[["Name", "Platform", "Year", "Global_Sales"]]


#this function calcuates what percentage of games belong to each genre
def genre_percentages(df):
    total_games = len(df) #total number of games in the dataset
    genre_counts = df["Genre"].value_counts() #counts how many times the genre appears

    percentages = (genre_counts / total_games) * 100 # changes the count to percentage
    
    # Create a DataFrame to display the results
    result = pd.DataFrame({
        "Genre": percentages.index,
        "Percentage": percentages.values
    })

    return result


# 3. finds the top 5 games for a platform based on global sales
def top_5_platform_games(df, platform_name):
    filtered = df[df["Platform"] == platform_name] #filters by platform

    sorted_games = filtered.sort_values(by="Global_Sales", ascending=False)

    top_5 = sorted_games.head(5).copy()

    # Convert millions to actual copies sold
    top_5["Copies_Sold"] = top_5["Global_Sales"] * 1_000_000

    top_5 = top_5.reset_index(drop=True)

    for i in range(len(top_5)):
        print(f"{i+1}. {top_5.loc[i, 'Name']} - {int(top_5.loc[i, 'Copies_Sold']):,} copies")

    return top_5[["Name", "Copies_Sold"]]


# Line chart of total global sales over time
def sales_over_time(df):
    yearly_sales = df.groupby("Year")["Global_Sales"].sum()

    plt.figure()
    yearly_sales.plot()

    plt.title("Total Video Game Sales Over Time")
    plt.xlabel("Year")
    plt.ylabel("Global Sales (Millions)")
    plt.show()


# This is the main part of the program
#runs only when the file is executed directly 
if __name__ == "__main__":
    df = load_data()

    print("\nGames by Nintendo:")
    print(games_by_publisher(df, "Nintendo"))

    print("\nGenre Percentages:")
    print(genre_percentages(df))

    print("\nTop 5 Wii Games:")
    top_5_platform_games(df, "Wii")

    sales_over_time(df)

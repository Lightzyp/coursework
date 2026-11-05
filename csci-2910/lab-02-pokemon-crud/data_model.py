from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import declarative_base

Base = declarative_base()

class Pokemon(Base):
    __tablename__ = "Pokemon"  

    ID = Column(Integer, primary_key=True, index=True)
    DexNumber = Column(Integer)
    Name = Column(String)
    Type1 = Column(String)
    Type2 = Column(String)
    Generation = Column(Integer)

    def __repr__(self):
        return (f"\nID: {self.ID}"
                f"\nDexNumber: {self.DexNumber}"
                f"\nName: {self.Name}"
                f"\nType1: {self.Type1}"
                f"\nType2: {self.Type2}"
                f"\nGeneration: {self.Generation}\n")
# Daniel_Macharia_InventoryLab

Text extracted from the completed coursework document. Screenshots, comments and document metadata are omitted.

Pseudocode

START
INPUT product nameINPUT current quantity in stockINPUT reorder pointINPUT unit priceINPUT high-priority status (yes/no)INPUT supplier availability (yes/no)
CALCULATE inventory value = quantity × unit price
IF quantity > reorder point THENSET stock status = "Stock Level OK"ELSE IF quantity = reorder point THENSET stock status = "Reorder Soon"ELSESET stock status = "Low Stock - Reorder Needed"END IF
SET reorder needed = quantity < reorder point
IF reorder needed AND high-priority status = "yes" THENSET reorder priority = "URGENT REORDER"ELSE IF reorder needed THENSET reorder priority = "NORMAL REORDER"ELSESET reorder priority = "NO REORDER NEEDED"END IF
IF reorder needed AND supplier availability = "yes" THENSET order status = "Reorder can be placed."ELSE IF reorder needed THENSET order status = "Reorder needed, but supplier is unavailable."ELSESET order status = "No reorder required at this time."END IF
DISPLAY report titleDISPLAY product nameDISPLAY current quantity in stockDISPLAY reorder pointDISPLAY unit price formatted to two decimal placesDISPLAY inventory value formatted to two decimal placesDISPLAY stock statusDISPLAY high-priority statusDISPLAY supplier availabilityDISPLAY reorder priorityDISPLAY order status as the final action
END


Screenshots

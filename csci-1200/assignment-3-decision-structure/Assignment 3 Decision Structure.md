# Assignment 3 Decision Structure

Text extracted from the completed coursework document. Screenshots, comments and document metadata are omitted.

Pseudocode:
START

Ask for the driver's name
Ask for campus status
Ask for parking hours
Ask if the driver has a valid permit

IF status is student OR employee
    IF permit is yes
        Set fee to 0
    ELSE
        Set fee to hours × 2
    END IF
ELSE IF status is visitor
    IF hours are 2 or less
        Set fee to hours × 3
    ELSE
        Set fee to hours × 5
    END IF
ELSE
    Display "Invalid status"
    STOP
END IF

Display the driver's name, status, hours, permit, and fee

IF fee is 0
    Display "Free parking"
ELSE
    Display "Payment required"
END IF

IF hours are more than 8
    Display a long-term parking warning
END IF

END

Test Runs:

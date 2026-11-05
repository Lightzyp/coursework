# Case Analysis

Text extracted from the completed coursework document. Screenshots, comments and document metadata are omitted.

Case Analysis
What information does the program need as input?
It needs a name, major, and workstation 2. What needs to be repeated for every student?
Name, Major, Workstation3. Where is a while loop needed, and why?
Validates the student and workshop count until the number entered is at 14. Where is a for loop needed, and why?
For loop processes each student and another displays each student’s workshops5. What will the nested loop represent?
The outer loop represents students, and the inner loop represents their workshops6. What value will be used as a running total (accumulator)?
Total_workshops adds up all workshop’s registration7. Why are functions useful in this program?
Functions organize the code, reduce repetitions, and make the program easier to read

Pseudocode

FUNCTION welcome_student(name)
    DISPLAY a welcome message using name
END FUNCTION

FUNCTION show_student_info(name, major, workshops)
    DISPLAY name, major, and workshops
END FUNCTION

FUNCTION goodbye_student(name)
    DISPLAY a check-in confirmation using name
END FUNCTION

FUNCTION main()
    SET total_workshops = 0

    INPUT students
    WHILE students < 1
        DISPLAY "Please enter at least 1 student."
        INPUT students
    END WHILE

    FOR student FROM 1 THROUGH students
        DISPLAY student number
        INPUT name
        INPUT major
        INPUT workshops

        WHILE workshops < 1
            DISPLAY "Please enter at least 1 workshop."
            INPUT workshops
        END WHILE

        CALL welcome_student(name)
        CALL show_student_info(name, major, workshops)

        FOR workshop FROM 1 THROUGH workshops
            DISPLAY "Registered for Workshop", workshop
        END FOR

        SET total_workshops = total_workshops + workshops
        CALL goodbye_student(name)
    END FOR

    DISPLAY "EVENT CHECK-IN SUMMARY"
    DISPLAY students
    DISPLAY total_workshops
END FUNCTION

CALL main()




Logic Explanation
The outer for loop processes each student and collects their name, major, and workshop count. The inner for loop displays a registration line for each workshop the student selects. A while loop keeps asking for the workshop count until it is at least 1. The welcome_student function displays a greeting, and total_workshops adds each student’s workshop count to produce the final total.

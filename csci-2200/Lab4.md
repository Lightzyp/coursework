# Lab4

Text extracted from the completed submission. Embedded screenshots/media are omitted from this public copy.

Lab Report – (Lab 4)

Name: DanielDate: 9/30/2025

Screen Shots

Figure 1

Figure 2

Figure 3

Figure 4

Figure 5

Figure 6

Figure 7

Figure 8

Figure 9

Figure 10

Figure 11

Figure 12

Figure 13

Figure 14

Figure 15

Figure 16

Figure 17

Research Questions

Questions (Exercises)

What is the first name of [REDACTED EMAIL]?

awk -F',' '$4=="[REDACTED EMAIL]"{print $2}' members2.txt

What company does member #53 work for?

awk -F',' '$1==53{print $5}' members2.txt

How many times does the name first name ‘Hayden’ appear in the file (one command, no

grep -c ",Hayden," members2.txt

pipes)? (Hint: this uses a grep option we talked about in lecture, but haven’t used in lab)

How many times does the ‘.com’ top level domain occur in the file?

grep -o "\.com" members2.txt | wc -l

members.txt (id;first_name;last_name;email;company_name;job_title)

How many members work for Tekfly?

grep -c ";Tekfly;" members.txt

Of the job titles in the file, which two appear most often?

cut -d';' -f6 members.txt | sort | uniq -c | sort -nr | head -2

7. Print the first 15 lines of members.txt (minus the first line) and replace the semicolons with ‘ - ‘

(space,dash,space), except between the first and last names, which should just have one space

random_data.txt (random_number, color, random_number, ip_address)

tail -n +2 members.txt | head -15 | awk -F';' '{printf "%s - %s %s - %s - %s - %s\n",$1,$2,$3,$4,$5,$6}'

How many times does the number 42 appear in the first field of the file?

grep -c "^42," random_data.txt

9. Find all of the entries whose color value is ‘Purple.’ Sort them on the third (random number)

field. What are the values of the Language and IP address fields for the first result?

grep ",Purple," random_data.txt | sort -t',' -k3,3n | awk -F',' 'NR==1{print $4, $5}'

10. Turn #8 into a bash script. Save the script as rand.sh. Make it executable and run it. Grab a

screen shot of the script (in nano or vim) and paste it into your lab submission (I don’t need the

commands you used here. Just the screen shot)

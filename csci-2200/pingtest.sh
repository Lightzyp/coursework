#!/bin/bash
########################################################
#  Name:        pingtest.sh
#  Author:      Daniel
#  Date:        2025-11-25
#  Last rev:    2025-11-25
#  Description: Connectivity test
########################################################

# colored text constants
RESET="\e[0m"
CYAN="\e[36m"
MAGENTA="\e[31m"
YELLOW="\e[38;5;228m"
PURPLE="\e[38;5;141m"
RED="\e[38;5;196m"
GREEN="\e[38;5;43m"
BLUE="\e[38;5;26m"
LTPURP="\e[38;5;219m"
ORANGE="\e[38;5;216m"

# Error check
if (( $# < 1 )); then
  echo
  echo -e "${RED} ! Error: Usage <pingtest.sh> <URL>${RESET}"
  echo
  exit 1
fi

clear
printf "${ORANGE}==== Checking %s ====\n${RESET}" "$1"
sleep 1s

printf "${RED} (Press Ctrl+C to quit)\n\n${RESET}"
count=1
printf "${CYAN}Trying %s ...\n${RESET}" "$1"

#===== Main =====
while true; do
   currtime=$(date +%H:%M:%S)
   sec=5

   ping -c1 "$1" &>/dev/null
   testval=$?

   # Success
   if (( testval == 0 )); then
     printf "${GREEN}\a %3s. (%s) %s available!\n\n${RESET}" "$count" "$currtime" "$1"
     exit 0
   else
     while (( sec-- > 0 )); do
       printf "${CYAN} %3s. (%s) No connection...Trying again in ${PURPLE}%s${RESET}\r" "$count" "$currtime" "$sec"
       sleep 1s
     done

     ((count++))
     printf "\n"
   fi
done

#===== Epilog =====
exit 0

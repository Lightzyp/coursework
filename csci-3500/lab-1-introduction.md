# Lab 1 Introduction

> Text extract of Daniel Macharia's coursework. Original source: DOCX. Embedded images and Office metadata are omitted from this public copy; sensitive identifiers and credential examples are redacted. Wording is preserved except for template placeholders and privacy cleanup.

CSCI 3500 – Spring 2026

Daniel Macharia

1/22/2026

Instructor: Ms. Emond

Lab Purpose (In Your Own Words)

The purpose of this lab was to introduce Kali Linux and to help me to become a little more familiar with its with the user interface and the built in security tools this lab focused on learn how to locate tools such ass Wireshark, Nmap, and Metasploit framework. Those told demonstrated hoe common tool are used for cybersecurity.

Tools & Resources Required

Kali Linux VM

Kali Linux was used as the primary operating system for this lab. It is a security focused Linux distribution that comes preinstall with many pen testing and network analysis tools

VMware Workstation player

VMware workstation is used to run kali Linux as a virtual machine on your computer

Terminal 

The terminal was used to run commands as ifconfig and nmap to gather system and network information

Wireshark

Wireshark is a network packet analyzer used to inspect and analyze network traffic. 

Nmap 

It is a network scanning tool used to identify open ports, services, and system information on a host.

Metasploit Framework

Metasploit us a pen testing framework that contains scanning and exploitation tools. In the lab it was explored to understand its interface and basic commands 

Lab Procedures

1. I began by launching VMware Workstation Player and starting the Kali Linux virtual machine. Once kali loaded, I logged in using default credentials provided for the system.

2. After logging in, I familiarized myself with the kali Linux desktop environment. I explored the applications menu located in the top-left-corner of the screen to view the different categories of security tools installed on the system

3. I used the search feature within the applications menu to locate Wireshark. Once found, I launched Wireshark to verify that the application was installed and functioning correctly. After confirming this, I closed the application.

4. Next, I opened the terminal from the top bar menu. Inside the terminal, I typed the ifconfig command to display network interface information. From the output, I identified and recorded the IP address

5. using the IP address, I performed a basic network scan by entering the nmap command followed by my Klie systems’ IP address. This scan identified open ports and basic service information on my own system

6. I then ran additional Nmap scans using different options I used -p- option to scall all ports and the -v option to enable verbose output, which provided more detailed scan information. I also attempted an OS detection scan using -A option.

7. After completing the Nmap scans, I opened the Metasploit Framework by navigating the Exploration tools category in the applications menu, when prompted, I entered my Kali password to access the Metasploit console.

8. Inside Metasploit, I used the db_nmap command to perform a network scan whole storing the results in the Metasploit database. I then used the hosts command to view discovered hosts.

9. I explored the available exploits by running the show exploits command and searched for window- related exploits using the search function. I did not run any exploits, as this lab only for us to get experience with the interface.

10. Throughout the lab I didn’t capture screen shots for all the steps but in later labs I will!

Observations

This lab helped me better understand how kali Linux works and structures. I learned how to navigate the application menu, use the terminal for command-line operations, and safely scan a system using Nmap. One important takeaway was seeing how the same tools can used for both defensive security and malicious purposes I also gained experience using Metasploit not a lot of experience but with this class in expecting to. Also, it was interesting to see all of the detailed and extensive list of available exploits is, even though they were not used in the lab. Overall, this lab helped me feel more comfortable using Kali Linux and gave me a strong foundation for future labs involving security testing and analysis.

Reflection Questions

How might a security professional use Kali Linux to improve the security posture of their organization

  A security professional can use Kali Linux to scan systems they own to identify open ports, running services, and potential vulnerabilities. Ny finding weaknesses early, they can apply patches, adjust configuration and strengthen defenses before attack

How might a threat actor use some of the tools in Kali Linux for malicious purposes?

A threat actor could use tools like Nmap and Metasploit to scan systems without permission, identify vulnerabilities, and exploit them to gain unauthorized access. This highlights why ethical use and proper use are critical when using security tools

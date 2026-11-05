# Firewall

> Text extract of Daniel Macharia's coursework. Original source: DOCX. Embedded images and Office metadata are omitted from this public copy; sensitive identifiers and credential examples are redacted. Wording is preserved except for template placeholders and privacy cleanup.

CSCI 3500 – Spring 2026 

Daniel Macharia

4/1/2026

Instructor: Ms. Emond

Firewall Lab

Lab Purpose (In Your Own Words)

The purpose of this lab was to understate how attackers discover vulnerable systems on a network and how firewall rules can be used to protect those systems. In this lab I used Kali to scan a local network and identify a vulnerable machine with multiple open ports. Then I demonstrated how unsecure protocols like Telent can expose sensitive information through packet sniffing.

Tools & Resources Required

Kali Linux VM – Used as the attacker machine to perform network scanning and packet sniffing.

Metasploitable 2 VM – Used as the vulnerable target server to demonstrate attacks and apply firewall rules.

Nmap – A network scanning tool used to discover hosts, open ports, and services on the network.

tcpdump – A packet sniffing tool used to capture and analyze network traffic.

Telnet – Used to demonstrate insecure remote access and plaintext data transmission.

iptables – A Linux firewall tool used to create and manage access control rules.

Lab Procedures

Set both VMs to host-only network.

Ran ifconfig on Kali to find IP and network range.

Used Nmap to scan network:

nmap -sS -sV 192.168.x.0/24

Found Metasploitable machine with open ports.

Opened two terminals in Kali.

Started packet sniffing:

sudo tcpdump -i eth0 -A port 23

Connected using Telnet:

telnet 192.168.x.x

Logged in and saw data appear in tcpdump (plaintext).

Closed sessions.

Logged into Metasploitable.

Checked firewall rules:

sudo iptables -L -n -v

Blocked Telnet:

sudo iptables -A INPUT -p tcp --dport 23 -j DROP

Verified rule and tested Telnet (failed).

Set default deny:

sudo iptables -P INPUT DROP

Ran Nmap again (ports filtered).

Allowed HTTPS:

sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT

Observations

I learned how easy it is to find open ports using Nmap. Telnet was very insecure because it showed usernames and commands in plain text. Using iptables made a big difference in security by blocking access. The default denying rule was the most important because it blocks everything unless allowed. Firewalls also reduce what attackers can see during scans.

Reflection Questions

1. Why use default deny?

It blocks all traffic unless allowed, making the system more secure and reducing risk.

Fr

2. Host-based vs next-gen firewalls?

Host-based firewalls protect one device (like iptables). Next-gen firewalls protect networks and have advanced features like intrusion detection.

3. How do firewalls affect attackers?

They limit information. Open ports show details, while filtered ports hide information and make attacks harder.

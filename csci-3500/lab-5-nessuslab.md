# Lab 5 NessusLab

> Text extract of Daniel Macharia's coursework. Original source: DOCX. Embedded images and Office metadata are omitted from this public copy; sensitive identifiers and credential examples are redacted. Wording is preserved except for template placeholders and privacy cleanup.

CSCI 3500 – Spring 2026

Daniel Macharia

February 26

Instructor: Ms. Emond

Nessus Lab

Lab Purpose (In Your Own Words)

This lab was for us to see the vulnerability of our network

Tools & Resources Required

                       Kali Linux VM

                       Metasploit able VM

                       Nessus Essentials

                      ifconfig and ping commands

                       Firefox browser (inside Kali)

Lab Procedures

First, I powered on the Metasploitable VM and used the ifconfig command to determine its IP address. Then, I powered on the Kali Linux VM and verified connectivity by pinging the Metasploitable machine.

Next, I downloaded and installed Nessus Essentials inside Kali Linux, started the Nessus service, and accessed the web interface through https://kali:8834/ . After registering with an activation code, I created a Basic Network Scan and entered the target IP address.

I launched the scan and reviewed the results once it was completed. Nessus displayed a list of vulnerabilities categorized by severity level.

Observations

This lab showed how automated scanning tools quickly identify system vulnerabilities and classify them by risk level (critical, High, Medium, Low, Informational). I learned how severity ratings help organizations prioritize remediation efforts and manage security risks effectively The Lab reinforced the importance of continuous monitoring patch management and vulnerability tracking in cybersecurity operations.

Reflection Questions

Reflection Questions:

1. How might a tool like Nessus help you to better track active vulnerabilities in yourorganization’s environment?

Nessus helps organizations automate vulnerability detection, generate, generate reports, and prioritize risk, making it easier to manage security across multiple systems.

2. How did Nessus categorize the vulnerabilities it discovered during the scan? What doesthe "severity" level mean, and how should it affect remediation decisions?

Severity levels indicate risk impact and exploitability. Critical and high vulnerabilities should be addressed first, while lower-level finding can be prioritized based on available resources.

3. Select one of the vulnerabilities found in your scan of the Metasploitable VM. See if youcan find some external research on that vulnerability. Are there any current patches for it?Are there other remediation steps recommended? 

An Outdated FTP service vulnerability was identified. In real world systems, this would be remediated by updating the software, applying patches, restricting access, or disabling unnecessary services,

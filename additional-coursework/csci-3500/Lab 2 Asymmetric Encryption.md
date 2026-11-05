# Asymmetric Encryption Lab

Completed CSCI 3500 Spring 2026 lab report by Daniel Macharia. Text extract; Office metadata is omitted. No screenshots, embedded objects, key material or literal credentials were found in the source.

CSCI 3500 – Spring 2026 Lab Template
Daniel Macharia
2/10/2026
Instructor: Ms. Emond

Asymmetric Encryption

Lab Purpose (In Your Own Words)
The purpose of this lab was to gain hands on experience with Asymmetric Encryption, also known as public key cryptography. This Lab demonstrated how public keys pairs are generated and how they are used to encrypt decrypt and digitally sign messages. It also showed how digital signatures verify authenticity and integrity. Overall, the lab helped me under how asymmetric encryption is used in real world secure communications such as email encryption and secure websites.


Tools & Resources Required

Kelopatra is a graphic user interface included with GPG4Win that allows users to manage certificates and perform encryption/decryption tasks.

Windows Operating SystemThe lab required a Windows environment to install and run GPG4Win.

D2L Discussion BoardUsed to share public keys with classmates and download peers’ public keys for encryption practice.


Lab Procedures (Minimum of 1 Page)

The Lab Procedures was pretty easy to do but hard to kind of put it in words, it would be easier to show or explain in person.

First you need the GPG Software - https://www.gpg4win.org/.You  have to select the download option, and you can skip the donation by selection $0. Download The executable file The run it as administrator then goes through the installation steps and select the recommended components choose a 

After installing, I launched Kleopatra top to create a new OpenPGP key pair. I clicked on “file” and selected “new OpenPGP key Pair” I entered my name and Etsu email address as I needed to. I checked the box to protect the generated key with a passphrase to ensure security. Next, I click on advance settings and select RSA 4096 for the key Material, m which provides strong encryption. I also set the key expiration date to one week or the date of creation, as instructed in the lab guidelines. After confirming the setting, I Clicked “OK” and the “CREATE”

I Then exported my public key by right clicking my certificate and selection “export”. I confirmed the file contained the PGP block
After that I downloaded my classmates public key and imported it into Kleopatra using the “import” option. I certified the key, entered my passphrase, and confirmed the peers’ name appeared in my certificate list. 

TO send a secure message I opened Kleopatra Notepad typed a secret message, m selected myself under “sign as” uncheck “encrypt for me” and selected my peer under “encrypt for other:” Icked “sign? Encrypt Notepad” and coped the encrypted message to send to my peers


Observations (Minimum of ½ a Page)

This Lab helped me better understand how asymmetric encryption works in practical way. Before this I understood the theory behind public and private keys but actually generating keys and encrypting messages made the process much cleaner. I learned that RSA 4096 creates a very strong key pair that would be extremely difficult to be brute force.

One important takeaway was understanding that public key can be shared freely without compromising security. Only the private key can decrypt messages encrypted with the public key. With a string pass phrase, because sone gains access to it they could decrypt message or impersonate the owner
I found the digital signature process particularly interesting. When signing a message It ensures that the message truly come from the sender and not been altered this adds both authenticity and integrity to communicate 

Reflection Questions

1. What is asymmetric cryptography?
Asymmetric cryptography uses two related keys: a public key and a private key. The public key encrypts messages, and the private key decrypts them. This allows secure communication without sharing a secret key first.

2. Why is it safe to share your public key?
It is safe because the public key cannot decrypt messages. Only the private key can do that. It is mathematically very difficult to figure out the private key from the public key.

3. Why keep a secure backup of your private key?
If you lose your private key, you lose access to your encrypted data. A secure backup allows recovery, but it must be protected to prevent unauthorized access.

4. What purpose does a digital signature serve?
A digital signature proves the message came from the sender and was not changed. It verifies authenticity and integrity.

5. What is a common use for asymmetric cryptography?
A common use is securing HTTPS websites. It is also used in encrypted email, digital certificates, and cryptocurrency.

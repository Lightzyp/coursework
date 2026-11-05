//Students share getFullName through the prototype. Course keeps enrollment private and throws an error if I try to add too many students

export function Student(firstName, lastName, email) {
    this.firstName = firstName;
    this.lastName = lastName;
    this.email = email;
}

Student.prototype.getFullName = function getFullName() {
    return `${this.firstName} ${this.lastName}`;
};

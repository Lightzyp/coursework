import { isValidCourseCode } from "../validators.js";

//LabCourse uses the regular course description and adds the room. The detached call loses this

export class Course {
    #code;
    #capacity;
    #enrolled;

    constructor(code, title, credits, capacity, enrolled = 0) {
        if (!isValidCourseCode(code)) {
            throw new RangeError(`Invalid course code: ${code}`);
        }
        this.#code = code;
        this.title = title;
        this.credits = credits;
        this.#capacity = Number(capacity);
        this.#enrolled = enrolled;
    }

    get code() {
        return this.#code;
    }

    get enrolled() {
        return this.#enrolled;
    }

    get openSeats() {
        return this.#capacity - this.#enrolled;
    }

    enroll(count = 1) {
        if (count > this.openSeats) {
            throw new RangeError(`${this.code} has only ${this.openSeats} open seat(s)`);
        }
        this.#enrolled += count;
    }

    describe() {
        return `${this.code}: ${this.title} (${this.credits} cr) ${this.enrolled}/${this.#capacity}`;
    }

    printRoster(students) {
        // The arrow keeps this set to the course.
        const printStudent = student => console.log(`${this.code}: ${student.getFullName()}`);
        students.forEach(printStudent);
    }
}

Course.prototype.isFull = function isFull() {
    return this.openSeats === 0;
};

export class LabCourse extends Course {
    #labRoom;

    constructor(code, title, credits, capacity, enrolled, labRoom) {
        super(code, title, credits, capacity, enrolled);
        this.#labRoom = labRoom;
    }

    get labRoom() {
        return this.#labRoom;
    }

    describe() {
        return `${super.describe()} | lab in ${this.labRoom}`;
    }
}

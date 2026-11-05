import { courses, announcement } from "./data.js";
import { isValidCourseCode, isValidEmail, extractCourseCodes } from "./validators.js";
import { getCapacity, getInstructor, isFull, describeCourse } from "./summary.js";
import {
    getCodes, getOpenCourses, getTotalCredits, getAverageEnrollment,
    getEmptiestCourse, getOpenThreeCreditTitles
} from "./stats.js";
import { createWaitlist, nextRequestId } from "./waitlist.js";
import { Student } from "./classes/Student.js";
import { Course, LabCourse } from "./classes/Course.js";

console.log("Checkpoint 1: Data");
console.log(`Loaded ${courses.length} courses`);
const firstCourse = courses[0];
const lastCourse = courses[courses.length - 1];
console.log(`${firstCourse.code} ${firstCourse.title}`);
console.log(`${lastCourse.code} ${lastCourse.title}`);
console.table(courses);

console.log("\nCheckpoint 2: Validation and loops");
let validCount = 0;
for (const course of courses) {
    if (isValidCourseCode(course.code)) {
        validCount += 1;
        continue;
    }
    console.log(`Invalid code: "${course.code}" (${course.title})`);
}
console.log(`${validCount} of ${courses.length} codes are valid`);

for (const key in firstCourse) {
    console.log(`${key}: ${firstCourse[key]}`);
}

console.log(extractCourseCodes(announcement));
console.log(isValidEmail("advising@etsu.edu"));
console.log(isValidEmail("advising@etsu"));

let courseIndex = 0;
while (courseIndex < courses.length) {
    if (courses[courseIndex].enrolled === 0) {
        console.log(`First course with no students: ${courses[courseIndex].code}`);
        break;
    }
    courseIndex += 1;
}

console.log("\nCheckpoint 3: Values, types, and comparison");
for (const course of courses) {
    if (typeof course.capacity === "string") {
        console.log(`${course.code} has capacity stored as a string`);
    }
    if (course.instructor === null) {
        console.log(`${course.code} has a null instructor`);
    }
    if (course.instructor === undefined) {
        console.log(`${course.code} has no instructor property`);
    }
}

// This compares how loose equality and strict equality treat a string capacity.
const capstone = courses[7];
console.log(capstone.capacity == 18);
console.log(capstone.capacity === 18);
console.log(getCapacity(capstone) === 18);
console.log(capstone.capacity + 2);
console.log(getCapacity(capstone) + 2);

const networking = courses[5];
console.log(`|| gives: ${networking.enrolled || "no data"}`);
console.log(`?? gives: ${networking.enrolled ?? "no data"}`);

console.log(getInstructor(courses[2]));
console.log(getInstructor(capstone));
console.log(isFull(courses[1]));
console.log(isFull(courses[0]));
courses.forEach(c => console.log(describeCourse(c)));

console.log("\nCheckpoint 4: Array methods");
console.log(getCodes(courses));
console.log(getOpenCourses(courses).map(c => c.code));
console.log(getTotalCredits(courses));
console.log(getAverageEnrollment(courses));
console.log(getEmptiestCourse(courses).code);
console.log(getOpenThreeCreditTitles(courses));

console.log("\nCheckpoint 5: Closures");
const webWaitlist = createWaitlist("CSCI 3110");
const csWaitlist = createWaitlist("CSCI 1260");
webWaitlist.add("Ada");
webWaitlist.add("Grace");
webWaitlist.add("Alan");
csWaitlist.add("Linus");
console.log(webWaitlist.describe());
console.log(csWaitlist.describe());
console.log(webWaitlist.next());
console.log(webWaitlist.size());
console.log(webWaitlist.names);
console.log(csWaitlist.next());
console.log(csWaitlist.next());
console.log(nextRequestId());
console.log(nextRequestId());
console.log(nextRequestId());

console.log("\nCheckpoint 6: Prototypes and classes");
const ada = new Student("Ada", "Lovelace", "ada@etsu.edu");
const grace = new Student("Grace", "Hopper", "grace@etsu.edu");
console.log(ada.getFullName());
console.log(ada.hasOwnProperty("firstName"));
console.log(ada.hasOwnProperty("getFullName"));
console.log(ada.getFullName === grace.getFullName);

const catalog = courses
    .filter(c => isValidCourseCode(c.code))
    .map(c => new Course(c.code, c.title, c.credits, c.capacity, c.enrolled));
console.log(`Created ${catalog.length} Course objects`);
catalog.forEach(c => console.log(c.describe()));

const os = catalog.find(c => c.code === "CSCI 4727");
os.enroll(2);
console.log(os.describe());
console.log(os.openSeats);
console.log(os.isFull());
try {
    os.enroll(20);
} catch (error) {
    console.log(`${error.name}: ${error.message}`);
}
try {
    new Course("csci 2210", "Data Structures", 3, 30);
} catch (error) {
    console.log(`${error.name}: ${error.message}`);
}
// console.log(os.#enrolled); // SyntaxError

console.log("\nCheckpoint 7: Inheritance and context binding");
const webLab = new LabCourse(
    "CSCI 3110", "Advanced Topics in Web Development", 3, 25, 23, "BC 130"
);
const mixed = [os, webLab];
mixed.forEach(c => console.log(c.describe()));
console.log(webLab instanceof LabCourse);
console.log(webLab instanceof Course);
console.log(os instanceof LabCourse);
webLab.enroll(2);
console.log(webLab.isFull());

webLab.printRoster([ada, grace]);

const detached = webLab.describe;
try {
    detached();
} catch (error) {
    console.log(`${error.name}: ${error.message}`);
}
const bound = webLab.describe.bind(webLab);
console.log(bound());
setTimeout(() => console.log(`Later: ${webLab.describe()}`), 0);

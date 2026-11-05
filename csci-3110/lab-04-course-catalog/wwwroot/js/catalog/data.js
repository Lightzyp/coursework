// data.js exports the courses. main.js imports them and prints the table.
export const courses = [
    { code: "CSCI 1250", title: "Introduction to Computer Science I",
      credits: 4, capacity: 30, enrolled: 28, instructor: "Patel" },
    { code: "CSCI 1260", title: "Introduction to Computer Science II",
      credits: 4, capacity: 30, enrolled: 30, instructor: "Nguyen" },
    { code: "CSCI 2150", title: "Computer Organization",
      credits: 3, capacity: "24", enrolled: 19, instructor: null },
    { code: "csci 2210", title: "Data Structures",
      credits: 3, capacity: 30, enrolled: 22, instructor: "Garcia" },
    { code: "CSCI 3110", title: "Advanced Topics in Web Development",
      credits: 3, capacity: 25, enrolled: 25, instructor: "Okafor" },
    { code: "CSCI 3400", title: "Networking Fundamentals",
      credits: 3, capacity: 20, enrolled: 0, instructor: "Kim" },
    { code: "MATH1910", title: "Calculus I",
      credits: 4, capacity: 35, enrolled: 31, instructor: "Lopez" },
    { code: "CSCI 4850", title: "Senior Capstone I",
      credits: 3, capacity: "18", enrolled: 12 },
    { code: "ENGL 1010", title: "Critical Reading and Expository Writing",
      credits: 3, capacity: 25, enrolled: 24, instructor: "Brooks" },
    { code: "CSCI 4727", title: "Operating Systems",
      credits: 3, capacity: 30, enrolled: 17, instructor: "Patel" }
];

export const announcement =
    "Before enrolling in CSCI 3110, complete CSCI 1260 and CSCI 2210. " +
    "MATH 1910 is recommended. Contact advising@etsu.edu with questions.";

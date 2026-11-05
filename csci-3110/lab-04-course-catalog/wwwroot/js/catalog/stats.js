//filter keeps items, map changes each item, reduce combines them, and sort puts them in order.

import { getCapacity, getOpenSeats } from "./summary.js";

export function getCodes(courses) {
    const getCode = course => course.code;
    return courses.map(getCode);
}

export function getOpenCourses(courses) {
    const hasOpenSeats = course => getOpenSeats(course) > 0;
    return courses.filter(hasOpenSeats);
}

export function getTotalCredits(courses) {
    return courses.reduce(function addCredits(total, course) {
        return total + course.credits;
    }, 0);
}

export function getAverageEnrollment(courses) {
    const total = courses.reduce(function addEnrollment(sum, course) {
        return sum + course.enrolled;
    }, 0);
    return total / courses.length;
}

export function getEmptiestCourse(courses) {
    return courses.reduce(function compareEnrollment(emptiest, course) {
        const currentRatio = course.enrolled / getCapacity(course);
        const lowestRatio = emptiest.enrolled / getCapacity(emptiest);
        return currentRatio < lowestRatio ? course : emptiest;
    });
}

export function getOpenThreeCreditTitles(courses) {
    const isOpenThreeCreditCourse = course => course.credits === 3 && getOpenSeats(course) > 0;
    const uppercaseTitle = course => course.title.toUpperCase();

    return courses
        .filter(isOpenThreeCreditCourse)
        .map(uppercaseTitle)
        .sort();
}

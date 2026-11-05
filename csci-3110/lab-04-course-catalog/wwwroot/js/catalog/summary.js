//converts capacity, calculates open seats, and uses TBA for missing instructors.

export function getCapacity(course) {
    return Number(course.capacity);
}

export function getOpenSeats(course) {
    return getCapacity(course) - course.enrolled;
}

export function isFull(course) {
    return getOpenSeats(course) === 0;
}

export function getInstructor(course) {
    return course.instructor ?? "TBA";
}

export function describeCourse(course) {
    return `${course.code} ${course.title} | ${getInstructor(course)} | ${course.enrolled}/${getCapacity(course)} | open: ${getOpenSeats(course)}`;
}

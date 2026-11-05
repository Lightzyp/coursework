export function createWaitlist(courseCode) {
    // Each waitlist keeps its own array inside this function.
   // waitlist.js keeps each list private and generates request IDs
    const names = [];

    return {
        add(name) {
            return names.push(name);
        },
        next() {
            return names.shift() ?? null;
        },
        size() {
            return names.length;
        },
        describe() {
            return `${courseCode} waitlist: ${names.length} waiting`;
        }
    };
}

export const nextRequestId = (function makeRequestIdGenerator() {
    let count = 0;

    return function getNextRequestId() {
        count += 1;
        return `REQ-${String(count).padStart(3, "0")}`;
    };
})();

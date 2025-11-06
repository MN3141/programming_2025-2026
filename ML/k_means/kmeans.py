class Point:
    def __init__(self,coordinates):
        self.coordinates = coordinates
        self.colour = ["155","155","155"]

class Example(Point):
    def __init__(self, coordinates):
        super().__init__(coordinates)
        self.assigned_centroid = None


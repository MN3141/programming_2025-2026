import PyQt6.QtWidgets as qwidgets
import PyQt6.QtCore as qcore
import pyqtgraph as pg
import time

globalCnt = 0

class MLWorker(qcore.QObject):
    resultReady = qcore.pyqtSignal(int)

    def __init__(self):
        super().__init__()
        self._running = True
        self._cnt = 0

    def doWork(self):
        """Simulate heavy computation."""
        for _ in range(100):
            time.sleep(1)
            self._cnt += 1
            self.resultReady.emit(self._cnt)

    def stop(self):
        self._running = False
        thread_handle = self.thread()
        thread_handle.quit()
        thread_handle.wait()

def fooSlot(label):
    global globalCnt

    label.setText(str(globalCnt))
    globalCnt += 1


def _guiSetup():
    guiApp = qwidgets.QApplication([])
    mainWindow = qwidgets.QWidget()
    mainWindow.setWindowTitle("FooQT")

    btnStart = qwidgets.QPushButton("Start Worker")
    fooBtn = qwidgets.QPushButton("Foo")
    btnStop = qwidgets.QPushButton("Stop Worker")
    cntLabel = qwidgets.QLabel("Count: 0")
    fooLabel = qwidgets.QLabel("Foo")
    plotWidget = pg.PlotWidget(title="ML Data")
    layout = qwidgets.QVBoxLayout()

    layout.addWidget(plotWidget)
    layout.addWidget(cntLabel)
    layout.addWidget(fooLabel)
    layout.addWidget(btnStart)
    layout.addWidget(btnStop)
    layout.addWidget(fooLabel)
    layout.addWidget(fooBtn)
    mainWindow.setLayout(layout)

    plotWidget.plot([1,2,3,4],[1,4,9,16])

    thread = qcore.QThread()
    worker = MLWorker()
    worker.moveToThread(thread)

    thread.started.connect(worker.doWork)
    worker.resultReady.connect(lambda v: cntLabel.setText(f"Count: {v}"))

    btnStop.clicked.connect(worker.stop)
    btnStart.clicked.connect(thread.start)
    fooBtn.clicked.connect(lambda: fooSlot(fooLabel))
    mainWindow.thread = thread
    mainWindow.worker = worker

    return guiApp, mainWindow


def main():

    guiApp, mainWindow = _guiSetup()
    mainWindow.show()
    guiApp.exec()

main()

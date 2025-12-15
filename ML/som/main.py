import os
import sys
import logging
import json
import numpy as np
import PyQt6.QtWidgets as qwidgets
import PyQt6.QtCore as qcore
import pyqtgraph as pg
from ml_worker import MLWorker
from point import Point

app_logger = None
configs = []


def _get_script_dir():

    script_path = sys.argv[0]
    script_dir = os.path.dirname(script_path)

    return script_dir


def _init_logger():

    global app_logger

    script_dir = _get_script_dir()
    log_file = os.path.join(script_dir, "som.log")
    app_logger = logging.getLogger(__name__)
    logging.basicConfig(
        filename=log_file,
        encoding="utf-8",
        level=logging.DEBUG,
        format="%(asctime)s %(message)s",
    )
    app_logger.info("Logger configured!")


def _get_input():

    global configs

    script_dir = _get_script_dir()
    config_file = os.path.join(script_dir, "configs.json")

    try:
        with open(config_file, "r") as f_handle:
            configs = json.load(f_handle)
    except Exception as e:
        app_logger.error(f"Could not read configs! {e}")
        sys.exit(1)

    app_logger.info("Input parsed!")


def _get_examples():

    global app_logger

    examples = []

    try:
        with open(configs["generated_points_path"], "r") as f_handle:
            file_data = f_handle.readlines()
    except Exception as e:
        app_logger.error(f"Could not read examples! {e}")
        sys.exit(1)

    for line in file_data:

        coordinates = line.split(":")[0]
        coordinates = coordinates[1:-1]
        coordinates = [float(num) for num in coordinates.split(",")]
        coordinates = np.array(coordinates)
        point = Point(coordinates)
        examples.append(point)

    return examples


def _init_ui():

    global app_logger, configs

    guiApp = qwidgets.QApplication([])
    mainWindow = qwidgets.QWidget()
    app_logger.info(f"Window obj addr: {hex(id(mainWindow))}")
    mainWindow.setWindowTitle("Self Organizing Maps")

    btnStep = qwidgets.QPushButton("Step")
    epochLabel = qwidgets.QLabel("Epochs: 0")
    learningLabel = qwidgets.QLabel("a(t): 0.7")
    neigbourLabel = qwidgets.QLabel("v(t): 6.7")
    plotWidget = pg.PlotWidget(title="SOM 2D")

    plotWidget.showGrid(x=True, y=True, alpha=0.3)
    neuron_plot = pg.ScatterPlotItem(size=10, pen=None, symbol="o", brush="red")
    plotWidget.addItem(neuron_plot)
    mainWindow.neurons_plot = neuron_plot
    layout = qwidgets.QVBoxLayout()

    layout.addWidget(plotWidget)
    layout.addWidget(btnStep)
    layout.addWidget(epochLabel)
    layout.addWidget(learningLabel)
    layout.addWidget(neigbourLabel)
    mainWindow.plotWidget = plotWidget
    mainWindow.setLayout(layout)

    mainWindow.btnStep = btnStep
    mainWindow.epochLabel = epochLabel
    mainWindow.learningLabel = learningLabel
    mainWindow.neigbourLabel = neigbourLabel

    app_logger.info("GUI set up!")
    return guiApp, mainWindow


def _init_worker(window_qt_obj, examples_list):

    global app_logger

    app_logger.info(f"Function window obj addr: {hex(id(window_qt_obj))}")
    thread_handle = qcore.QThread()
    worker = MLWorker(
        configs["epochs_no"], examples_list, configs["domains"], configs["grid_no"]
    )
    worker.moveToThread(thread_handle)

    thread_handle.start()
    window_qt_obj.btnStep.clicked.connect(worker.stepEpochSlot)
    worker.resultReady.connect(
        lambda epoch, positions, learning_rate, neighbour_radius: _update_neuron_plot(
            window_qt_obj, epoch, positions, learning_rate, neighbour_radius
        )
    )

    window_qt_obj.thread = thread_handle
    window_qt_obj.worker = worker


def _plot_examples(examples, plot_widget):

    examples_plot = pg.ScatterPlotItem(size=10, pen=None, symbol="x")
    x_vals = [p.coordinates[0] for p in examples]
    y_vals = [p.coordinates[1] for p in examples]

    examples_plot.setData(x=x_vals, y=y_vals, brush="blue")
    plot_widget.addItem(examples_plot)


def _update_neuron_plot(
    window_qt_obj, epoch, neuron_positions, learning_rate, neighbour
):

    window_qt_obj.epochLabel.setText(f"Epochs: {epoch}")
    window_qt_obj.learningLabel.setText(f"a(t): {learning_rate}")
    window_qt_obj.neigbourLabel.setText(f"v(t): {neighbour}")
    x_vals = neuron_positions[:, 0]
    y_vals = neuron_positions[:, 1]
    window_qt_obj.neurons_plot.setData(x=x_vals, y=y_vals)


def main():

    _init_logger()
    _get_input()

    examples = _get_examples()
    gui_app, main_window = _init_ui()

    app_logger.info(f"Window obj addr: {hex(id(main_window))}")
    _plot_examples(examples, main_window.plotWidget)

    _init_worker(main_window, examples)
    main_window.show()
    gui_app.exec()


main()

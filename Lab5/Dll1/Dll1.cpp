#include "stdafx.h"
#include <iostream>
#include <fstream>
#include <vector>
#include <ctime>
#include <chrono>

struct DiagMatrix {
	int order;
	std::vector<double> data;
public:
	DiagMatrix(int order = 1) : order(order) {
		for (int i = 0; i < order; ++i) {
			data.push_back(0.0);
		}
	}

	DiagMatrix(int order, std::vector<double> &data_in) : order(order), data(data_in) {
		for (int i = 0; i < order; ++i) {
			data.push_back(data_in[i]);
		}
	}

	DiagMatrix(const DiagMatrix &other) {
		order = other.order;
		data = std::vector<double>(other.data);
	}

	void printMatrix(std::ofstream &fout) {
		try {
			for (int i = 0; i < order; ++i) {
				for (int j = 0; j < order; ++j) {
					if (i != j) fout << 0.0 << " ";
					else fout << data[i] << " ";
				}
				fout << std::endl;
			}
			fout << std::endl;
		}
		catch (...) {
			throw 1;
		}
	}

	DiagMatrix operator+(const DiagMatrix &other) const {
		try {
			if (order != other.order) throw 1;
			DiagMatrix res(order);
			for (int i = 0; i < other.order; ++i) {
				res.data[i] = data[i] + other.data[i];
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	DiagMatrix operator-(const DiagMatrix &other) const {
		try {
			if (order != other.order) throw 1;
			DiagMatrix res(order);
			for (int i = 0; i < other.order; ++i) {
				res.data[i] = data[i] - other.data[i];
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	DiagMatrix operator*(const DiagMatrix &other) const {
		try {
			if (order != other.order) throw 1;
			DiagMatrix res(order);
			for (int i = 0; i < other.order; ++i) {
				res.data[i] = data[i] * other.data[i];
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	std::vector<double> operator*(const std::vector<double> &other) const {
		try {
			if (order != other.size()) throw 1;
			std::vector<double> res;
			for (int i = 0; i < other.size(); ++i) {
				res.push_back(data[i] * other[i]);
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	static DiagMatrix inverse(const DiagMatrix &other) {
		try {
			DiagMatrix res(other.order);
			for (int i = 0; i < other.order; ++i) {
				if (other.data[i] == 0) throw 1;
				res.data[i] = 1.0 / other.data[i];
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

};

struct BlockMatrix {
	int N; //blocks
	std::vector<DiagMatrix> mainDiag, upDiag, downDiag;
public:
	BlockMatrix(int blocks = 2, int smallOrder = 1) : N(blocks) {
		downDiag.push_back(DiagMatrix(smallOrder));
		for (int i = 0; i < blocks - 1; ++i) {
			mainDiag.push_back(DiagMatrix(smallOrder));
			upDiag.push_back(DiagMatrix(smallOrder));
			downDiag.push_back(DiagMatrix(smallOrder));
		}
		mainDiag.push_back(DiagMatrix(smallOrder));
	}

	void setMainDiag(const std::vector<DiagMatrix> &other) {
		try {
			for (int i = 0; i < N; ++i) {
				mainDiag[i] = other[i];
			}
		}
		catch (...) {
			throw 1;
		}
	}

	void setUpDiag(const std::vector<DiagMatrix> &other) {
		try {
			for (int i = 0; i < N - 1; ++i) {
				upDiag[i] = other[i];
			}
		}
		catch (...) {
			throw 1;
		}
	}

	void setDownDiag(const std::vector<DiagMatrix> &other) {
		try {
			for (int i = 1; i < N; ++i) {
				downDiag[i] = other[i];
			}
		} 
		catch (...) {
			throw 1;
		}
	}

	void printBlockMatrix(std::ofstream &fout) {
		try {
			fout << "Main diag:" << std::endl;
			for (int i = 0; i < N; ++i) {
				mainDiag[i].printMatrix(fout);
			}
			fout << "Up diag:" << std::endl;
			for (int i = 0; i < N - 1; ++i) {
				upDiag[i].printMatrix(fout);
			}
			fout << "Down diag:" << std::endl;
			for (int i = 1; i < N; ++i) {
				downDiag[i].printMatrix(fout);
			}
		}
		catch (...) {
			throw 1;
		}
	}

	static std::vector<double> sum(const std::vector<double> &v1, const std::vector<double> &v2) {
		try {
			if (v1.size() != v2.size()) throw 1;
			std::vector<double> res;
			for (int i = 0; i < v1.size(); ++i) {
				res.push_back(v1[i] + v2[i]);
			}
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	static std::vector<double> diff(std::vector<double> v1, std::vector<double> v2) {
		try {
			for (int i = 0; i < v2.size(); ++i) {
				v2[i] *= -1;
			}
			return sum(v1, v2);
		}
		catch (...) {
			throw 1;
		}
	}

	std::vector<std::vector<double>> operator*(const std::vector<std::vector<double>> &v) const {
		try {
			if (v.size() != N) throw 1;
			std::vector<std::vector<double>> res;

			std::vector<double> tmp1 = mainDiag[0] * v[0];
			std::vector<double> tmp2 = upDiag[0] * v[1], tmp3;
			res.push_back(sum(tmp1, tmp2));

			for (int i = 1; i < N - 1; ++i) {
				tmp1 = downDiag[i] * v[i - 1];
				tmp2 = mainDiag[i] * v[i];
				tmp3 = upDiag[i] * v[i + 1];
				res.push_back(sum(sum(tmp1, tmp2), tmp3));
			}

			tmp1 = downDiag[N - 1] * v[N - 2];
			tmp2 = mainDiag[N - 1] * v[N - 1];
			res.push_back(sum(tmp1, tmp2));
			return res;
		}
		catch (...) {
			throw 1;
		}
	}

	std::vector<std::vector<double>> solve(const std::vector<std::vector<double>> &f) {
		try {
			if (N == 1) {
				DiagMatrix d = DiagMatrix::inverse(mainDiag[0]);
				std::vector<std::vector<double>> solution;
				std::vector<double> v = d * f[0];
				solution.push_back(v);
				return solution;
			}
			std::vector<DiagMatrix> alpha(N);
			std::vector<std::vector<double>> beta(N + 1);
			alpha[1] = DiagMatrix::inverse(mainDiag[0]) * upDiag[0];
			beta[1] = DiagMatrix::inverse(mainDiag[0]) * f[0];
			for (int i = 1; i < N - 1; ++i) {
				DiagMatrix tmp = DiagMatrix::inverse(mainDiag[i] - downDiag[i] * alpha[i]);
				alpha[i + 1] = tmp * upDiag[i];
				beta[i + 1] = tmp * diff(f[i], downDiag[i] * beta[i]);
			}
			beta[N] = DiagMatrix::inverse(mainDiag[N - 1] - downDiag[N - 1] * alpha[N - 1]) * diff(f[N - 1], downDiag[N - 1] * beta[N - 1]);

			std::vector<std::vector<double>> x(N);
			x[N - 1] = beta[N];
			for (int i = N - 2; i >= 0; --i) {
				x[i] = diff(beta[i + 1], alpha[i + 1] * x[i + 1]);
			}
			return x;
		}
		catch (...) {
			throw 1;
		}
	}

	std::vector<std::vector<double>> solveAndSave(const std::vector<std::vector<double>> &f, std::ofstream &fout) {
		try {
			std::vector<std::vector<double>> x = solve(f);
			printBlockMatrix(fout);

			fout << "Right part:" << std::endl;
			for (int i = 0; i < f.size(); ++i) {
				for (int j = 0; j < f[i].size(); ++j) {
					fout << f[i][j] << " ";
				}
			}
			fout << std::endl << std::endl;

			fout << "Solution:" << std::endl;
			for (int i = 0; i < x.size(); ++i) {
				for (int j = 0; j < x[i].size(); ++j) {
					fout << x[i][j] << std::endl;
				}
			}
			fout << std::endl;
			fout.close();
			return x;
		}
		catch (...) {
			throw 1;
		}
	}
};

extern "C" _declspec(dllexport) void GlobalExportFunction(
	double *mainD,
	double *upD,
	double *downD,
	double *fD,
	double *solution_ret,
	int blocks,
	int smallOrder,
	double *time,
	const char *filename,
	bool save) {

	std::vector<DiagMatrix> mainDiag;
	for (int i = 0; i < blocks; ++i) {
		std::vector<double> data(mainD + i * smallOrder, mainD + (i + 1) * smallOrder);
		DiagMatrix d(smallOrder, data);
		mainDiag.push_back(d);
	}

	std::vector<DiagMatrix> upDiag;
	for (int i = 0; i < blocks - 1; ++i) {
		std::vector<double> data(upD + i * smallOrder, upD + (i + 1) * smallOrder);
		DiagMatrix d(smallOrder, data);
		upDiag.push_back(d);
	}

	std::vector<DiagMatrix> downDiag;
	downDiag.push_back(0);
	for (int i = 0; i < blocks - 1; ++i) {
		std::vector<double> data(downD + i * smallOrder, downD + (i + 1) * smallOrder);
		DiagMatrix d(smallOrder, data);
		downDiag.push_back(d);
	}

	std::vector<std::vector<double>> f;
	for (int i = 0; i < blocks; ++i) {
		std::vector<double> data(fD + i * smallOrder, fD + (i + 1) * smallOrder);
		f.push_back(data);
	}

	BlockMatrix b(blocks, smallOrder);
	b.setMainDiag(mainDiag);
	b.setUpDiag(upDiag);
	b.setDownDiag(downDiag);

	std::vector<std::vector<double>> solution;

	auto start = std::chrono::steady_clock::now();
	if (save) {
		std::ofstream fout(filename);
		solution = b.solveAndSave(f, fout);
		fout.close();
	} else solution = b.solve(f);
	auto finish = std::chrono::steady_clock::now();
	*time = (double) std::chrono::duration_cast<std::chrono::microseconds>(finish - start).count() / 1000;

	for (int i = 0; i < blocks; ++i) {
		for (int j = 0; j < smallOrder; ++j) {
			solution_ret[i * smallOrder + j] = solution[i][j];
		}
	}
}